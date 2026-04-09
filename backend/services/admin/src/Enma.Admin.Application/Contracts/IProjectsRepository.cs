using System.Text.Json.Nodes;
using Enma.Admin.Application.Dto.Super;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for domain <see cref="Project"/>.
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// (e.g. <c>NotFoundError</c>) when the entity does not exist.
/// </summary>
public interface IProjectsRepository
{
    /// <summary>Creates a new project.</summary>
    Task<Result<Project>> CreateAsync(Project project, CancellationToken ct = default);

    /// <summary>Gets a project by id.</summary>
    Task<Result<Project>> GetByIdAsync(Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Gets a project by (orgId, key).</summary>
    Task<Result<Project>> GetByOrgAndKeyAsync(Guid orgId, string key, CancellationToken ct = default);

    /// <summary>Lists projects for an organization (paged by offset/limit).</summary>
    Task<Result<IReadOnlyList<Project>>> ListByOrgAsync(Guid orgId, int offset, int limit, CancellationToken ct = default);

    /// <summary>
    /// Lists projects available to the user (requires project memberships, otherwise may be implemented as org projects).
    /// </summary>
    Task<Result<IReadOnlyList<Project>>> ListByUserAsync(Guid userId, int offset, int limit, CancellationToken ct = default);

    /// <summary>Persists a full project update (when a fully loaded domain model is available).</summary>
    Task<Result> UpdateAsync(Project project, CancellationToken ct = default);

    /// <summary>Updates <see cref="Project.Name"/> only.</summary>
    Task<Result> SetNameAsync(Guid projectId, Guid orgId, string name, CancellationToken ct = default);

    /// <summary>Updates <see cref="Project.Description"/> only. Pass null to clear the value.</summary>
    Task<Result> SetDescriptionAsync(Guid projectId, Guid orgId, string? description, CancellationToken ct = default);

    /// <summary>
    /// Updates <see cref="Project.Settings"/> only. Pass null to clear the value.
    /// Stored as jsonb on persistence side.
    /// </summary>
    Task<Result> SetSettingsAsync(Guid projectId, Guid orgId, JsonObject? settings, CancellationToken ct = default);

    /// <summary>Archives a project (sets ArchivedAt).</summary>
    Task<Result> SetArchivedAsync(Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Unarchives a project (clears ArchivedAt).</summary>
    Task<Result> ClearArchivedAsync(Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Soft-deletes a project (sets DeletedAt).</summary>
    Task<Result> SoftDeleteAsync(Guid projectId, Guid orgId, CancellationToken ct = default);

    /// <summary>Counts non-deleted projects in an organization.</summary>
    Task<Result<int>> CountByOrgAsync(Guid orgId, CancellationToken ct = default);

    // ---------- Super-admin (platform-wide) ----------

    /// <summary>Platform-wide paged list of projects as list-item projections.</summary>
    Task<Result<IReadOnlyList<SuperProjectListItemDto>>> ListSuperAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        Guid? organizationId,
        CancellationToken ct = default);

    /// <summary>Platform-wide count of projects matching filters.</summary>
    Task<Result<int>> CountSuperAsync(
        string? search,
        bool includeDeleted,
        Guid? organizationId,
        CancellationToken ct = default);

    /// <summary>Super-admin details view for a single project (includes org context, members, definitions counts).</summary>
    Task<Result<SuperProjectDetailsDto>> GetSuperDetailsAsync(Guid projectId, CancellationToken ct = default);

    /// <summary>Counts all projects (optionally including soft-deleted).</summary>
    Task<Result<int>> CountAllAsync(bool includeDeleted, CancellationToken ct = default);
}