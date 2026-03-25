using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

/// <summary>
/// Repository for organization invites (<see cref="OrganizationInvite"/>).
/// Implementations should not return null; return a failed <see cref="Result"/> with a typed domain error
/// when the entity does not exist.
/// </summary>
public interface IOrganizationInvitesRepository
{
    /// <summary>Creates a new invite.</summary>
    Task<Result<OrganizationInvite>> CreateAsync(OrganizationInvite invite, CancellationToken ct = default);

    /// <summary>Gets an invite by id.</summary>
    Task<Result<OrganizationInvite>> GetByIdAsync(Guid inviteId, Guid orgId, CancellationToken ct = default);

    /// <summary>Gets an active invite for the given org/email (not accepted, not declined, not expired).</summary>
    Task<Result<OrganizationInvite>> GetActiveByOrgAndEmailAsync(Guid orgId, string email, CancellationToken ct = default);

    /// <summary>Lists active invites for an organization (paged by offset/limit).</summary>
    Task<Result<IReadOnlyList<OrganizationInvite>>> ListActiveByOrgAsync(Guid orgId, int offset, int limit, CancellationToken ct = default);

    /// <summary>Lists pending invites for a user by email (not accepted, not declined, not expired).</summary>
    Task<Result<IReadOnlyList<OrganizationInvite>>> ListPendingByEmailAsync(string email, int offset, int limit, CancellationToken ct = default);

    /// <summary>Marks an invite as accepted and links it to a user id (AccountId).</summary>
    Task<Result> SetAcceptedAsync(Guid inviteId, Guid orgId, Guid acceptedUserId, CancellationToken ct = default);

    /// <summary>Marks an invite as declined and links it to a user id (AccountId).</summary>
    Task<Result> SetDeclinedAsync(Guid inviteId, Guid orgId, Guid declinedUserId, CancellationToken ct = default);
}
