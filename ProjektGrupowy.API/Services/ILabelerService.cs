﻿using ProjektGrupowy.API.DTOs.Labeler;
using ProjektGrupowy.API.Models;
using ProjektGrupowy.API.Utils;

namespace ProjektGrupowy.API.Services;

public interface ILabelerService
{
    Task<Optional<IEnumerable<Labeler>>> GetLabelersAsync();
    Task<Optional<Labeler>> GetLabelerAsync(int id);
    Task<Optional<Labeler>> AddLabelerAsync(LabelerRequest labelerRequest);
    Task<Optional<Labeler>> UpdateLabelerAsync(int labelerId, LabelerRequest labelerRequest);
    Task DeleteLabelerAsync(int id);
    Task<Optional<Labeler>> AddLabelerWithUser(LabelerRequest labelerRequest, User user);
    Task<Optional<Labeler>> GetLabelerByUserIdAsync(string userId);
    Task<Optional<IEnumerable<Labeler>>> GetLabelersByProjectAsync(int projectId);
}