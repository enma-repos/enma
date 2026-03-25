using Enma.Common.Enums;

namespace Enma.Admin.Application.Dto.Notifications;

public sealed record CreateNotificationDto(
    Guid RecipientUserId,
    NotificationType Type,
    string Title,
    string Message,
    Guid? ResourceId = null);
