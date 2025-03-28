﻿using System.ComponentModel.DataAnnotations;

namespace ProjektGrupowy.API.DTOs.AccessCode;

public class CreateAccessCodeRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }

    [Required]
    public DateTime? ExpiresAtUtc { get; set; }
}