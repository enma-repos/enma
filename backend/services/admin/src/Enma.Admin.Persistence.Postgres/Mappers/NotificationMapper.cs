using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class NotificationMapper
{
    internal static Notification ToModel(this NotificationEntity entity)
        => Notification.Rehydrate(
            id: entity.Id,
            recipientUserId: entity.RecipientUserId,
            type: entity.Type,
            title: entity.Title,
            message: entity.Message,
            resourceId: entity.ResourceId,
            isRead: entity.IsRead,
            createdAt: entity.CreatedAt,
            readAt: entity.ReadAt);

    internal static NotificationEntity ToEntity(this Notification model)
        => new()
        {
            Id = model.Id,
            RecipientUserId = model.RecipientUserId,
            Type = model.Type,
            Title = model.Title,
            Message = model.Message,
            ResourceId = model.ResourceId,
            IsRead = model.IsRead,
            CreatedAt = model.CreatedAt,
            ReadAt = model.ReadAt
        };
}
