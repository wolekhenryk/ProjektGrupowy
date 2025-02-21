﻿using ProjektGrupowy.API.Models;
using ProjektGrupowy.API.Utils;

namespace ProjektGrupowy.API.Repositories;

public interface ISubjectRepository
{
    Task<Optional<IEnumerable<Subject>>> GetSubjectsAsync();
    Task<Optional<Subject>> GetSubjectAsync(int id);
    Task<Optional<Subject>> AddSubjectAsync(Subject subject);
    Task<Optional<Subject>> UpdateSubjectAsync(Subject subject);

    Task<Optional<IEnumerable<Subject>>> GetSubjectsByProjectAsync(int projectId);
    Task<Optional<IEnumerable<Subject>>> GetSubjectsByProjectAsync(Project project);

    Task DeleteSubjectAsync(Subject subject);

    Task<Optional<IEnumerable<Subject>>> GetSubjectsByScientistId(int scientistId);
}