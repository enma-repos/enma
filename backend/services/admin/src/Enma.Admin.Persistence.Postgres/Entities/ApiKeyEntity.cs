namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class ApiKeyEntity
{
    public Guid Id { get; set; }

    public Guid SdkClientId { get; set; }
    public SdkClientEntity? SdkClient { get; set; }
    
    public required string KeyPrefix { get; set; }
    public required string KeyHash { get; set; }

    public long SentEventsCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}