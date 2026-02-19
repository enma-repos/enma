using System.Text.Json.Nodes;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for domain <see cref="Organization"/>.
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// (e.g. <c>NotFoundError</c>) when the entity does not exist.
/// </summary>
public interface IOrganizationsRepository
{
    /// <summary>Creates a new organization.</summary>
    Task<Result<Organization>> CreateAsync(Organization org, CancellationToken ct = default);

    /// <summary>Gets an organization by id.</summary>
    Task<Result<Organization>> GetByIdAsync(Guid orgId, CancellationToken ct = default);

    /// <summary>Gets an organization by slug.</summary>
    Task<Result<Organization>> GetBySlugAsync(string slug, CancellationToken ct = default);

    /// <summary>
    /// Lists organizations where the user is a member/owner.
    /// The result window is defined by <paramref name="offset"/> and <paramref name="limit"/>.
    /// </summary>
    Task<Result<IReadOnlyList<Organization>>> ListByUserAsync(Guid userId, int offset, int limit, CancellationToken ct = default);

    /// <summary>Persists a full organization update (when a fully loaded domain model is available).</summary>
    Task<Result> UpdateAsync(Organization org, CancellationToken ct = default);

    /// <summary>Updates <see cref="Organization.Name"/> only.</summary>
    Task<Result> SetNameAsync(Guid orgId, string name, CancellationToken ct = default);
    
    /// <summary> Updates <see cref="Organization.OwnerUserId"/> only.</summary>
    Task<Result> SetOwnerAsync(Guid orgId, Guid ownerUserId, CancellationToken ct = default);

    /// <summary>Soft-deletes an organization (sets DeletedAt).</summary>
    Task<Result> SoftDeleteAsync(Guid orgId, CancellationToken ct = default);
}