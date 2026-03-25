using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.Notifications;

public sealed record NotificationDto(
    Guid Id,
    Guid RecipientUserId,
    NotificationType Type,
    string Title,
    string Message,
    Guid? ResourceId,
    bool IsRead,
    DateTime CreatedAt,
    DateTime? ReadAt);
