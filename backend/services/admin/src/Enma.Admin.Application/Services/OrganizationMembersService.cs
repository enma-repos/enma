using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Notifications;
using Enma.Admin.Application.Dto.OrganizationMembers;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationMembersService : IOrganizationMembersService
{
    private readonly IOrganizationMembersRepository _organizationMembersRepository;
    private readonly INotificationsService _notificationsService;

    public OrganizationMembersService(
        IOrganizationMembersRepository organizationMembersRepository,
        INotificationsService notificationsService)
    {
        _organizationMembersRepository = organizationMembersRepository;
        _notificationsService = notificationsService;
    }

    public async Task<Result<OrganizationMemberDto>> GetAsync(
        Guid orgId,
        Guid userId,
        CancellationToken ct = default)
    {
        var res = await _organizationMembersRepository.GetAsync(orgId, userId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationMemberDto>(res.Errors);
    }

    public Task<Result<bool>> IsMemberAsync(Guid orgId, Guid userId, CancellationToken ct = default)
        => _organizationMembersRepository.IsMemberAsync(orgId, userId, ct);

    public async Task<Result<IReadOnlyList<OrganizationMemberDto>>> ListByOrgAsync(
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var res = await _organizationMembersRepository.ListByOrgAsync(orgId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<OrganizationMemberDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<OrganizationMemberDto>>(res.Errors);
    }

    public async Task<Result> SetRoleAsync(
        Guid orgId,
        Guid userId,
        SetOrganizationMemberRoleDto dto,
        CancellationToken ct = default)
    {
        var res = await _organizationMembersRepository.SetRoleAsync(orgId, userId, dto.Role, ct);
        if (res.IsFailed)
        {
            return Result.Fail(res.Errors);
        }

        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: userId,
            Type: NotificationType.OrganizationRoleChanged,
            Title: "Organization role changed",
            Message: $"Your organization role has been changed to {dto.Role}.",
            ResourceId: orgId), ct);

        return Result.Ok();
    }

    public async Task<Result> SetStatusAsync(
        Guid orgId,
        Guid userId,
        SetOrganizationMemberStatusDto dto,
        CancellationToken ct = default)
    {
        var res = await _organizationMembersRepository.SetStatusAsync(orgId, userId, dto.Status, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> RemoveAsync(Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var res = await _organizationMembersRepository.RemoveAsync(orgId, userId, ct);
        if (res.IsFailed)
        {
            return Result.Fail(res.Errors);
        }

        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: userId,
            Type: NotificationType.RemovedFromOrganization,
            Title: "Removed from organization",
            Message: "You have been removed from an organization.",
            ResourceId: orgId), ct);

        return Result.Ok();
    }
}
