namespace Enma.Auth.Persistence.Postgres.Entities;

internal sealed class RefreshTokenEntity
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }
    public required AccountEntity Account { get; set; }
    
    public required string TokenHash { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public DateTime LastUsedAt { get; set; }
}