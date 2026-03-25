using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

public interface INotificationsRepository
{
    Task<Result<Notification>> CreateAsync(Notification notification, CancellationToken ct = default);

    Task<Result<IReadOnlyList<Notification>>> ListByRecipientAsync(
        Guid recipientUserId, bool unreadOnly, int offset, int limit, CancellationToken ct = default);

    Task<Result<int>> GetUnreadCountAsync(Guid recipientUserId, CancellationToken ct = default);

    Task<Result> SetReadAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default);

    Task<Result> SetAllReadAsync(Guid recipientUserId, CancellationToken ct = default);

    Task<Result> DeleteAsync(Guid notificationId, Guid recipientUserId, CancellationToken ct = default);
}
