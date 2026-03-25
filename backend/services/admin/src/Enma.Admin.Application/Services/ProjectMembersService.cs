using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Notifications;
using Enma.Admin.Application.Dto.ProjectMembers;
using Enma.Admin.Application.Models;
using Enma.Common.Enums;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class ProjectMembersService : IProjectMembersService
{
    private readonly IProjectMembersRepository _projectMembersRepository;
    private readonly IOrganizationMembersRepository _organizationMembersRepository;
    private readonly INotificationsService _notificationsService;

    public ProjectMembersService(
        IProjectMembersRepository projectMembersRepository,
        IOrganizationMembersRepository organizationMembersRepository,
        INotificationsService notificationsService)
    {
        _projectMembersRepository = projectMembersRepository;
        _organizationMembersRepository = organizationMembersRepository;
        _notificationsService = notificationsService;
    }

    public async Task<Result<ProjectMemberDto>> AddAsync(
        Guid organizationId,
        AddProjectMemberDto dto,
        CancellationToken ct = default)
    {
        var isMemberRes = await _organizationMembersRepository.IsMemberAsync(organizationId, dto.UserId, ct);
        if (isMemberRes.IsFailed)
        {
            return Result.Fail<ProjectMemberDto>(isMemberRes.Errors);
        }

        if (!isMemberRes.Value)
        {
            return Result.Fail<ProjectMemberDto>(
                ApplicationErrors.Validation("User is not a member of this organization."));
        }

        var now = DateTime.UtcNow;

        var modelRes = ProjectMember.Create(
            projectId: dto.ProjectId,
            userId: dto.UserId,
            role: dto.Role,
            joinedAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<ProjectMemberDto>(modelRes.Errors);
        }

        var res = await _projectMembersRepository.AddAsync(modelRes.Value, ct);
        if (res.IsFailed)
        {
            return Result.Fail<ProjectMemberDto>(res.Errors);
        }
        
        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: dto.UserId,
            Type: NotificationType.AddedToProject,
            Title: "Added to project",
            Message: "You have been added to a project.",
            ResourceId: dto.ProjectId), ct);

        return Result.Ok(res.Value.ToDto());
    }

    public async Task<Result<ProjectMemberDto>> GetAsync(
        Guid projectId,
        Guid userId,
        CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.GetAsync(projectId, userId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProjectMemberDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ProjectMemberDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.ListByProjectAsync(projectId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ProjectMemberDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ProjectMemberDto>>(res.Errors);
    }

    public async Task<Result> SetRoleAsync(
        Guid projectId,
        Guid userId,
        SetProjectMemberRoleDto dto,
        CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.SetRoleAsync(projectId, userId, dto.Role, ct);
        if (res.IsFailed)
        {
            return Result.Fail(res.Errors);
        }

        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: userId,
            Type: NotificationType.ProjectRoleChanged,
            Title: "Project role changed",
            Message: $"Your project role has been changed to {dto.Role}.",
            ResourceId: projectId), ct);

        return Result.Ok();
    }

    public async Task<Result> RemoveAsync(Guid projectId, Guid userId, CancellationToken ct = default)
    {
        var res = await _projectMembersRepository.RemoveAsync(projectId, userId, ct);
        if (res.IsFailed)
        {
            return Result.Fail(res.Errors);
        }

        await _notificationsService.CreateAsync(new CreateNotificationDto(
            RecipientUserId: userId,
            Type: NotificationType.RemovedFromProject,
            Title: "Removed from project",
            Message: "You have been removed from a project.",
            ResourceId: projectId), ct);

        return Result.Ok();
    }
}
