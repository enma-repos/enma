using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Notifications;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class NotificationsService : INotificationsService
{
    private readonly INotificationsRepository _notificationsRepository;

    public NotificationsService(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }

    public async Task<Result<NotificationDto>> CreateAsync(
        CreateNotificationDto dto,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var modelRes = Notification.Create(
            recipientUserId: dto.RecipientUserId,
            type: dto.Type,
            title: dto.Title,
            message: dto.Message,
            resourceId: dto.ResourceId,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<NotificationDto>(modelRes.Errors);
        }

        var res = await _notificationsRepository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<NotificationDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<NotificationDto>>> ListByRecipientAsync(
        Guid recipientUserId, bool unreadOnly, int offset, int limit,
        CancellationToken ct = default)
    {
        var res = await _notificationsRepository.ListByRecipientAsync(recipientUserId, unreadOnly, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<NotificationDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<NotificationDto>>(res.Errors);
    }

    public Task<Result<int>> GetUnreadCountAsync(Guid recipientUserId, CancellationToken ct = default)
        => _notificationsRepository.GetUnreadCountAsync(recipientUserId, ct);

    public Task<Result> SetReadAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default)
        => _notificationsRepository.SetReadAsync(notificationId, recipientUserId, ct);

    public Task<Result> SetAllReadAsync(Guid recipientUserId, CancellationToken ct = default)
        => _notificationsRepository.SetAllReadAsync(recipientUserId, ct);

    public Task<Result> DeleteAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default)
        => _notificationsRepository.DeleteAsync(notificationId, recipientUserId, ct);
}
