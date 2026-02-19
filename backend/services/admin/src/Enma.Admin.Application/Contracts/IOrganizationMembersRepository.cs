using Enma.Admin.Application.Models;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for organization membership records (<see cref="OrganizationMember"/>).
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// (e.g. <c>NotFoundError</c>) when the entity does not exist.
/// </summary>
public interface IOrganizationMembersRepository
{
    /// <summary>Adds a member to an organization.</summary>
    Task<Result<OrganizationMember>> AddAsync(OrganizationMember member, CancellationToken ct = default);

    /// <summary>Gets a membership record by (orgId, userId).</summary>
    Task<Result<OrganizationMember>> GetAsync(Guid orgId, Guid userId, CancellationToken ct = default);

    /// <summary>Checks whether a user is a member of the organization.</summary>
    Task<Result<bool>> IsMemberAsync(Guid orgId, Guid userId, CancellationToken ct = default);

    /// <summary>Lists members of an organization (paged by offset/limit).</summary>
    Task<Result<IReadOnlyList<OrganizationMember>>> ListByOrgAsync(Guid orgId, int offset, int limit, CancellationToken ct = default);

    /// <summary>Updates the member role only.</summary>
    Task<Result> SetRoleAsync(Guid orgId, Guid userId, OrganizationRole role, CancellationToken ct = default);

    /// <summary>Updates the member status only.</summary>
    Task<Result> SetStatusAsync(Guid orgId, Guid userId, OrganizationMemberStatus status, CancellationToken ct = default);

    /// <summary>Removes a member from the organization.</summary>
    Task<Result> RemoveAsync(Guid orgId, Guid userId, CancellationToken ct = default);
}