using Enma.Common.Enums;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class NotificationEntity
{
    public Guid Id { get; set; }

    public Guid RecipientUserId { get; set; }
    public UserEntity? RecipientUser { get; set; }

    public NotificationType Type { get; set; }

    public required string Title { get; set; }
    public required string Message { get; set; }

    public Guid? ResourceId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}
