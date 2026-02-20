using Enma.Admin.Application.Dto.OrganizationMembers;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IOrganizationMembersService
{
    Task<Result<OrganizationMemberDto>> GetAsync(Guid orgId, Guid userId, CancellationToken ct = default);
    Task<Result<bool>> IsMemberAsync(Guid orgId, Guid userId, CancellationToken ct = default);

    Task<Result<IReadOnlyList<OrganizationMemberDto>>> ListByOrgAsync(
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetRoleAsync(
        Guid orgId,
        Guid userId,
        SetOrganizationMemberRoleDto dto,
        CancellationToken ct = default);

    Task<Result> SetStatusAsync(
        Guid orgId,
        Guid userId,
        SetOrganizationMemberStatusDto dto,
        CancellationToken ct = default);

    Task<Result> RemoveAsync(Guid orgId, Guid userId, CancellationToken ct = default);
}
