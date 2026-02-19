using Enma.Common.Enums;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class OrganizationInviteEntity
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }
    public OrganizationEntity? Organization { get; set; }
    
    public required string TargetEmail { get; set; }
    public OrganizationRole Role { get; set; }
    
    public required string TokenHash { get; set; }

    public DateTime ExpiresAt { get; set; }

    public Guid CreatedByUserId { get; set; }
    public UserEntity? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    
    public Guid? AcceptedUserId { get; set; }
    public UserEntity? AcceptedUser { get; set; }
}