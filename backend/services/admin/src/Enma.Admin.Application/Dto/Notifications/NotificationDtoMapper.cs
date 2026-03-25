using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.Notifications;

internal static class NotificationDtoMapper
{
    internal static NotificationDto ToDto(this Notification model)
        => new(
            Id: model.Id,
            RecipientUserId: model.RecipientUserId,
            Type: model.Type,
            Title: model.Title,
            Message: model.Message,
            ResourceId: model.ResourceId,
            IsRead: model.IsRead,
            CreatedAt: model.CreatedAt,
            ReadAt: model.ReadAt);
}
