﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektGrupowy.API.DTOs.LabelerAssignment;
using ProjektGrupowy.API.DTOs.Project;
using ProjektGrupowy.API.DTOs.Subject;
using ProjektGrupowy.API.DTOs.SubjectVideoGroupAssignment;
using ProjektGrupowy.API.DTOs.VideoGroup;
using ProjektGrupowy.API.Filters;
using ProjektGrupowy.API.Models;
using ProjektGrupowy.API.Services;
using ProjektGrupowy.API.Utils.Constants;
using System.Security.Claims;
using ProjektGrupowy.API.DTOs.Labeler;

namespace ProjektGrupowy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(ValidateModelStateFilter))]
[Authorize]
public class ProjectController(
    IProjectService projectService, 
    ISubjectService subjectService, 
    IVideoGroupService videoGroupService, 
    ISubjectVideoGroupAssignmentService subjectVideoGroupAssignmentService, 
    ILabelerService labelerService, 
    IAuthorizationHelper authHelper,
    IMapper mapper) : ControllerBase
{
    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetProjectsAsync()
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        var projects = checkResult.IsAdmin
            ? await projectService.GetProjectsAsync()
            : await projectService.GetProjectsOfScientist(checkResult.Scientist!.Id);

        return projects.IsSuccess
            ? Ok(mapper.Map<IEnumerable<ProjectResponse>>(projects.GetValueOrThrow()))
            : NotFound(projects.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponse>> GetProjectAsync(int id)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, id);
            if (authResult != null)
            {
                return authResult;
            }
        }

        var project = await projectService.GetProjectAsync(id);
        return project.IsSuccess
            ? Ok(mapper.Map<ProjectResponse>(project.GetValueOrThrow()))
            : NotFound(project.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutProjectAsync(int id, ProjectRequest projectRequest)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, id);
            if (authResult != null)
            {
                return authResult;
            }
            
            projectRequest.ScientistId = checkResult.Scientist!.Id;
        }

        var updateResult = await projectService.UpdateProjectAsync(id, projectRequest);
        return updateResult.IsSuccess
            ? NoContent()
            : BadRequest(updateResult.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> PostProject(ProjectRequest projectRequest)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            projectRequest.ScientistId = checkResult.Scientist!.Id;
        }

        var projectResult = await projectService.AddProjectAsync(projectRequest);
        if (!projectResult.IsSuccess)
        {
            return BadRequest(projectResult.GetErrorOrThrow());
        }

        var createdProject = projectResult.GetValueOrThrow();

        return CreatedAtAction("GetProject", new { id = createdProject.Id },
            mapper.Map<ProjectResponse>(createdProject));
    }

    [HttpPost("join")]
    public async Task<IActionResult> AssignLabelerToGroupAssignment(LabelerAssignmentDto laveAssignmentDto)
    {
        if (!User.IsInRole(RoleConstants.Labeler))
        {
            return Forbid();
        }

        var labelerOpt = await labelerService.GetLabelerByUserIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        var labeler = labelerOpt.GetValueOrThrow() ?? throw new InvalidOperationException("Labeler not found.");

        laveAssignmentDto.LabelerId = labeler.Id;

        var result = await projectService.AddLabelerToProjectAsync(laveAssignmentDto);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpPost("{projectId:int}/distribute")]
    public async Task<IActionResult> DistributeLabelersEqually(int projectId)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, projectId);
            if (authResult != null)
            {
                return authResult;
            }
        }

        var result = await projectService.DistributeLabelersEquallyAsync(projectId);
        return result.IsSuccess 
            ? Ok(result.GetValueOrThrow())
            : NotFound(result.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, id);
            if (authResult != null)
            {
                return authResult;
            }
        }

        await projectService.DeleteProjectAsync(id);
        return NoContent();
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpGet("{projectId:int}/Subjects")]
    public async Task<ActionResult<IEnumerable<SubjectResponse>>> GetSubjectsByProjectAsync(int projectId)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, projectId);
            if (authResult != null)
            {
                return authResult;
            }
        }

        var subjectsResult = await subjectService.GetSubjectsByProjectAsync(projectId);
        return subjectsResult.IsSuccess 
            ? Ok(mapper.Map<IEnumerable<SubjectResponse>>(subjectsResult.GetValueOrThrow()))
            : NotFound(subjectsResult.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpGet("{projectId:int}/VideoGroups")]
    public async Task<ActionResult<IEnumerable<VideoGroupResponse>>> GetVideoGroupsByProjectAsync(int projectId)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, projectId);
            if (authResult != null)
            {
                return authResult;
            }
        }

        var videoGroupsResult = await videoGroupService.GetVideoGroupsByProjectAsync(projectId);
        return videoGroupsResult.IsSuccess 
            ? Ok(mapper.Map<IEnumerable<VideoGroupResponse>>(videoGroupsResult.GetValueOrThrow()))
            : NotFound(videoGroupsResult.GetErrorOrThrow());
    }

    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpGet("{projectId:int}/SubjectVideoGroupAssignments")]
    public async Task<ActionResult<IEnumerable<SubjectVideoGroupAssignmentResponse>>> GetSubjectVideoGroupAssignmentsByProjectAsync(int projectId)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, projectId);
            if (authResult != null)
            {
                return authResult;
            }
        }

        var assignmentsResult = await subjectVideoGroupAssignmentService.GetSubjectVideoGroupAssignmentsByProjectAsync(projectId);
        return assignmentsResult.IsSuccess 
            ? Ok(mapper.Map<IEnumerable<SubjectVideoGroupAssignmentResponse>>(assignmentsResult.GetValueOrThrow()))
            : NotFound(assignmentsResult.GetErrorOrThrow());
    }
    
    [Authorize(Policy = PolicyConstants.RequireAdminOrScientist)]
    [HttpGet("{projectId:int}/Labelers")]
    public async Task<ActionResult<IEnumerable<LabelerResponse>>> GetLabelersByProjectAsync(int projectId)
    {
        var checkResult = await authHelper.CheckGeneralAccessAsync(User);
        if (checkResult.Error != null)
        {
            return checkResult.Error;
        }

        if (checkResult.IsScientist)
        {
            var authResult = await authHelper.EnsureScientistOwnsProjectAsync(User, projectId);
            if (authResult != null)
            {
                return authResult;
            }
        }

        var labelersResult = await labelerService.GetLabelersByProjectAsync(projectId);
        return labelersResult.IsSuccess 
            ? Ok(mapper.Map<IEnumerable<LabelerResponse>>(labelersResult.GetValueOrThrow()))
            : NotFound(labelersResult.GetErrorOrThrow());
    }
}