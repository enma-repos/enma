using Enma.Common.Enums;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid RecipientUserId { get; private set; }

    public NotificationType Type { get; private set; }

    public string Title { get; private set; } = null!;
    public string Message { get; private set; } = null!;

    public Guid? ResourceId { get; private set; }

    public bool IsRead { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { }

    private Notification(Guid id, Guid recipientUserId, NotificationType type, string title, string message,
        Guid? resourceId, DateTime createdAt)
    {
        Id = id;
        RecipientUserId = recipientUserId;
        Type = type;
        Title = title;
        Message = message;
        ResourceId = resourceId;
        CreatedAt = createdAt;
    }

    public static Result<Notification> Create(Guid recipientUserId, NotificationType type, string title,
        string message, Guid? resourceId, DateTime createdAt)
    {
        return Result.Ok(new Notification(
            Guid.NewGuid(), recipientUserId, type, title, message, resourceId, createdAt));
    }

    public static Notification Rehydrate(Guid id, Guid recipientUserId, NotificationType type, string title,
        string message, Guid? resourceId, bool isRead, DateTime createdAt, DateTime? readAt)
    {
        return new Notification
        {
            Id = id,
            RecipientUserId = recipientUserId,
            Type = type,
            Title = title,
            Message = message,
            ResourceId = resourceId,
            IsRead = isRead,
            CreatedAt = createdAt,
            ReadAt = readAt
        };
    }
}
