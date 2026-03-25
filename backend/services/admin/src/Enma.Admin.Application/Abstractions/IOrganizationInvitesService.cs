using Enma.Admin.Application.Dto.OrganizationInvites;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IOrganizationInvitesService
{
    Task<Result<OrganizationInviteDto>> CreateAsync(CreateOrganizationInviteDto dto, CancellationToken ct = default);
    Task<Result<OrganizationInviteDto>> GetByIdAsync(Guid inviteId, Guid orgId, CancellationToken ct = default);

    Task<Result<OrganizationInviteDto>> GetActiveByOrgAndEmailAsync(
        Guid orgId,
        string email,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<OrganizationInviteDto>>> ListActiveByOrgAsync(
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<OrganizationInviteDto>>> ListPendingByEmailAsync(
        Guid currentUserId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetAcceptedAsync(
        Guid inviteId,
        Guid orgId,
        SetInviteAcceptedDto dto,
        CancellationToken ct = default);

    Task<Result> SetDeclinedAsync(
        Guid inviteId,
        Guid orgId,
        SetInviteDeclinedDto dto,
        CancellationToken ct = default);
}
