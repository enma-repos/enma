namespace Enma.Auth.Persistence.Postgres.Entities;

internal sealed class ExternalAuthEntity
{
    public required string Provider { get; set; }
    public required string Subject { get; set; }

    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; } = null!;

    public DateTime LinkedAt { get; set; }
}