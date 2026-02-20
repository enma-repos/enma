using Enma.Admin.Application.Dto.OrganizationInvites;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IOrganizationInvitesService
{
    Task<Result<OrganizationInviteDto>> CreateAsync(CreateOrganizationInviteDto dto, CancellationToken ct = default);
    Task<Result<OrganizationInviteDto>> GetByIdAsync(Guid inviteId, CancellationToken ct = default);

    Task<Result<OrganizationInviteDto>> GetActiveByOrgAndEmailAsync(
        Guid orgId,
        string email,
        CancellationToken ct = default);

    Task<Result<IReadOnlyList<OrganizationInviteDto>>> ListActiveByOrgAsync(
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetAcceptedAsync(
        Guid inviteId,
        SetInviteAcceptedDto dto,
        CancellationToken ct = default);
}
