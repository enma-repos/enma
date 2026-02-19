using Enma.Admin.Application.Models;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for project membership records (<see cref="ProjectMember"/>).
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// when the entity does not exist.
/// </summary>
public interface IProjectMembersRepository
{
    /// <summary>Adds a member to a project.</summary>
    Task<Result<ProjectMember>> AddAsync(ProjectMember member, CancellationToken ct = default);

    /// <summary>Gets a membership record by (projectId, userId).</summary>
    Task<Result<ProjectMember>> GetAsync(Guid projectId, Guid userId, CancellationToken ct = default);

    /// <summary>Lists members of a project (paged by offset/limit).</summary>
    Task<Result<IReadOnlyList<ProjectMember>>> ListByProjectAsync(Guid projectId, int offset, int limit, CancellationToken ct = default);

    /// <summary>Updates the member role only.</summary>
    Task<Result> SetRoleAsync(Guid projectId, Guid userId, ProjectRole role, CancellationToken ct = default);

    /// <summary>Removes a member from the project.</summary>
    Task<Result> RemoveAsync(Guid projectId, Guid userId, CancellationToken ct = default);
}
