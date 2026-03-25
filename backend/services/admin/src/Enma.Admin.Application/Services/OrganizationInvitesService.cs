using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Notifications;
using Enma.Admin.Application.Dto.OrganizationInvites;
using Enma.Admin.Application.Models;
using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationInvitesService : IOrganizationInvitesService
{
    private const int InviteLifeTimeInDays = 30;

    private readonly IOrganizationInvitesRepository _organizationInvitesRepository;
    private readonly IOrganizationMembersRepository _organizationMembersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly INotificationsService _notificationsService;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationInvitesService(
        IOrganizationInvitesRepository organizationInvitesRepository,
        IOrganizationMembersRepository organizationMembersRepository,
        IUsersRepository usersRepository,
        INotificationsService notificationsService,
        IUnitOfWork unitOfWork)
    {
        _organizationInvitesRepository = organizationInvitesRepository;
        _organizationMembersRepository = organizationMembersRepository;
        _usersRepository = usersRepository;
        _notificationsService = notificationsService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrganizationInviteDto>> CreateAsync(
        CreateOrganizationInviteDto dto,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now + TimeSpan.FromDays(InviteLifeTimeInDays);

        var modelRes = OrganizationInvite.Create(
            id: Guid.NewGuid(),
            orgId: dto.OrganizationId,
            email: dto.TargetEmail,
            role: dto.Role,
            createdByUserId: dto.CreatedByUserId,
            acceptedUserId: null,
            createdAt: now,
            expiresAt: expiresAt,
            acceptedAt: null);

        if (modelRes.IsFailed)
        {
            return Result.Fail<OrganizationInviteDto>(modelRes.Errors);
        }

        var res = await _organizationInvitesRepository.CreateAsync(modelRes.Value, ct);
        if (res.IsFailed)
        {
            return Result.Fail<OrganizationInviteDto>(res.Errors);
        }

        return Result.Ok(res.Value.ToDto());
    }

    public async Task<Result<OrganizationInviteDto>> GetByIdAsync(
        Guid inviteId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _organizationInvitesRepository.GetByIdAsync(inviteId, orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationInviteDto>(res.Errors);
    }

    public async Task<Result<OrganizationInviteDto>> GetActiveByOrgAndEmailAsync(
        Guid orgId,
        string email,
        CancellationToken ct = default)
    {
        var res = await _organizationInvitesRepository.GetActiveByOrgAndEmailAsync(orgId, email, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationInviteDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<OrganizationInviteDto>>> ListActiveByOrgAsync(
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var res = await _organizationInvitesRepository.ListActiveByOrgAsync(orgId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<OrganizationInviteDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<OrganizationInviteDto>>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<OrganizationInviteDto>>> ListPendingByEmailAsync(
        Guid currentUserId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var userRes = await _usersRepository.GetByIdAsync(currentUserId, ct);
        if (userRes.IsFailed)
        {
            return Result.Fail<IReadOnlyList<OrganizationInviteDto>>(userRes.Errors);
        }

        var res = await _organizationInvitesRepository.ListPendingByEmailAsync(userRes.Value.Email, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<OrganizationInviteDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<OrganizationInviteDto>>(res.Errors);
    }

    public async Task<Result> SetAcceptedAsync(
        Guid inviteId,
        Guid orgId,
        SetInviteAcceptedDto dto,
        CancellationToken ct = default)
    {
        var inviteRes = await _organizationInvitesRepository.GetByIdAsync(inviteId, orgId, ct);
        if (inviteRes.IsFailed)
        {
            return Result.Fail(inviteRes.Errors);
        }

        var invite = inviteRes.Value;
        var now = DateTime.UtcNow;
        Result? failure = null;

        await _unitOfWork.ExecuteInTransactionAsync(async innerCt =>
        {
            var acceptRes = await _organizationInvitesRepository.SetAcceptedAsync(inviteId, orgId, dto.AcceptedUserId, innerCt);
            if (acceptRes.IsFailed)
            {
                failure = acceptRes;
                throw new OperationCanceledException();
            }

            var memberRes = OrganizationMember.Create(invite.OrganizationId, dto.AcceptedUserId, invite.Role, now);
            if (memberRes.IsFailed)
            {
                failure = Result.Fail(memberRes.Errors);
                throw new OperationCanceledException();
            }

            var addRes = await _organizationMembersRepository.AddAsync(memberRes.Value, innerCt);
            if (addRes.IsFailed)
            {
                failure = Result.Fail(addRes.Errors);
                throw new OperationCanceledException();
            }
        }, ct);

        if (failure is not null)
        {
            return failure;
        }

        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: invite.CreatedByUserId,
            Type: NotificationType.OrganizationInviteAccepted,
            Title: "Invite accepted",
            Message: "Your organization invite has been accepted.",
            ResourceId: invite.OrganizationId), ct);

        return Result.Ok();
    }

    public async Task<Result> SetDeclinedAsync(
        Guid inviteId,
        Guid orgId,
        SetInviteDeclinedDto dto,
        CancellationToken ct = default)
    {
        var inviteRes = await _organizationInvitesRepository.GetByIdAsync(inviteId, orgId, ct);
        if (inviteRes.IsFailed)
        {
            return Result.Fail(inviteRes.Errors);
        }

        var invite = inviteRes.Value;

        var res = await _organizationInvitesRepository.SetDeclinedAsync(inviteId, orgId, dto.DeclinedUserId, ct);
        if (res.IsFailed)
        {
            return res;
        }

        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: invite.CreatedByUserId,
            Type: NotificationType.OrganizationInviteDeclined,
            Title: "Invite declined",
            Message: "Your organization invite has been declined.",
            ResourceId: invite.OrganizationId), ct);

        return Result.Ok();
    }
}
