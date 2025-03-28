﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjektGrupowy.API.Data;
using ProjektGrupowy.API.DTOs.Auth;
using ProjektGrupowy.API.DTOs.Labeler;
using ProjektGrupowy.API.DTOs.Scientist;
using ProjektGrupowy.API.Filters;
using ProjektGrupowy.API.Models;
using ProjektGrupowy.API.Services;
using ProjektGrupowy.API.Utils.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjektGrupowy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(ValidateModelStateFilter))] // ValidateModelStateFilter is a custom filter to validate model state
[Authorize]
public class AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
                                AppDbContext context, IConfiguration configuration, IScientistService scientistService, ILabelerService labelerService) : ControllerBase
{
    private readonly string JwtCookieName = configuration["JWT:JwtCookieName"];

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var userExists = await userManager.FindByNameAsync(model.UserName);
        if (userExists != null)
            return StatusCode(500, new { Status = "Error", Message = "User already exists!" });

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        await using var transaction = await context.Database.BeginTransactionAsync();
        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return StatusCode(500, new { Status = "Error", Message = "User creation failed!", Errors = errors });
        }

        if (!await roleManager.RoleExistsAsync(model.Role))
            return StatusCode(500, new { Status = "Error", Message = "Role does not exist!" });

        var roleResult = await userManager.AddToRoleAsync(user, model.Role);
        if (!roleResult.Succeeded)
            return StatusCode(500, new { Status = "Error", Message = "Failed to assign role to user." });

        if (model.Role == RoleConstants.Scientist)
        {
            var scientistRequest = new ScientistRequest
            {
                FirstName = model.UserName,
                LastName = model.UserName
            };

            var scientistResult = await scientistService.AddScientistWithUser(scientistRequest, user);

            if (scientistResult.IsFailure)
                return BadRequest(scientistResult.GetErrorOrThrow());

            var createdScientist = scientistResult.GetValueOrThrow();
            user.Scientist = createdScientist;

            await context.SaveChangesAsync();
        }
        else if (model.Role == RoleConstants.Labeler)
        {
            var labelerRequest = new LabelerRequest
            {
                Name = model.UserName
            };

            var labelerResult = await labelerService.AddLabelerWithUser(labelerRequest, user);

            if (labelerResult.IsFailure)
                return BadRequest(labelerResult.GetErrorOrThrow());

            var createdLabeler = labelerResult.GetValueOrThrow();
            user.Labeler = createdLabeler;

            await context.SaveChangesAsync();
        }

        await transaction.CommitAsync();

        return Ok(new { Status = "Success", Message = "User created and assigned to role successfully!" });
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await userManager.FindByNameAsync(model.UserName);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized(new { Status = "Error", Message = "Invalid UserName or password!" });

        var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        var userRoles = await userManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = GenerateJwtToken(authClaims);

        // Ustawienie tokenu jako ciasteczko HTTP Only
        Response.Cookies.Append(JwtCookieName, new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Używaj tylko w przypadku HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = token.ValidTo
        });

        return Ok(new
        {
            Message = "Login successful!",
            ExpiresAt = token.ValidTo // Zwróć czas wygaśnięcia tokena
        });
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        // Usuń ciasteczko JWT
        Response.Cookies.Delete(JwtCookieName);
        return Ok(new { Message = "Logout successful!" });
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var jwtToken = Request.Cookies[JwtCookieName];
        if (string.IsNullOrEmpty(jwtToken))
            return Unauthorized(new { Status = "Error", Message = "No token provided." });

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetTokenValidationParameters();

        var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);

        if (!(validatedToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return Unauthorized(new { Status = "Error", Message = "Invalid token." });
        }

        var user = await userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null)
            return Unauthorized(new { Status = "Error", Message = "User not found." });

        var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        var userRoles = await userManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var newToken = GenerateJwtToken(authClaims);

        Response.Cookies.Append(JwtCookieName, new JwtSecurityTokenHandler().WriteToken(newToken), new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = newToken.ValidTo
        });

        return Ok(new
        {
            Message = "Token refreshed successfully!",
            ExpiresAt = newToken.ValidTo
        });
    }

    [HttpGet("VerifyToken")]
    public async Task<IActionResult> VerifyToken()
    {
        var jwtToken = Request.Cookies[JwtCookieName];
        if (string.IsNullOrEmpty(jwtToken))
            return Unauthorized(new { IsAuthenticated = false });

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetTokenValidationParameters();

        var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);

        if (!(validatedToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return Unauthorized(new { IsAuthenticated = false });
        }

        var user = await userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null)
            return Unauthorized(new { IsAuthenticated = false });

        return Ok(new
        {
            IsAuthenticated = true,
            Username = user.UserName,
            Roles = await userManager.GetRolesAsync(user)
        });
    }

    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:ValidIssuer"],
            ValidAudience = configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
            ClockSkew = TimeSpan.Zero // Brak tolerancji dla czasu wygaśnięcia
        };
    }

    private JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(6),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
