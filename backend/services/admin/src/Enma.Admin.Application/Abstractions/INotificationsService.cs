using Enma.Admin.Application.Dto.Notifications;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface INotificationsService
{
    Task<Result<NotificationDto>> CreateAsync(CreateNotificationDto dto, CancellationToken ct = default);

    Task<Result<IReadOnlyList<NotificationDto>>> ListByRecipientAsync(
        Guid recipientUserId, bool unreadOnly, int offset, int limit, CancellationToken ct = default);

    Task<Result<int>> GetUnreadCountAsync(Guid recipientUserId, CancellationToken ct = default);

    Task<Result> SetReadAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default);

    Task<Result> SetAllReadAsync(Guid recipientUserId, CancellationToken ct = default);

    Task<Result> DeleteAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default);
}
