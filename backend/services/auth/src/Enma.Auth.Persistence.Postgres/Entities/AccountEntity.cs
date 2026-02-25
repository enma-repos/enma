using Enma.Common.Enums;

namespace Enma.Auth.Persistence.Postgres.Entities;

internal sealed class AccountEntity
{
    public Guid Id { get; set; }
    
    public required string Email { get; set; }
    public AccountStatus Status { get; set; }
    
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    
    public DateTime LastLoginAt { get; set; }
    
    public DateTime OnboardingStartedAt { get; set; }
    public DateTime? OnboardingCompletedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<ExternalAuthEntity> ExternalAuths { get; set; } = new List<ExternalAuthEntity>();
}