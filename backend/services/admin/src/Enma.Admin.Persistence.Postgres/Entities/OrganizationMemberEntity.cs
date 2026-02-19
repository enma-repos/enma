using Enma.Common.Enums;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class OrganizationMemberEntity
{
    public Guid OrganizationId { get; set; }
    public OrganizationEntity? Organization { get; set; }

    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    public OrganizationRole Role { get; set; }
    public OrganizationMemberStatus Status { get; set; } = OrganizationMemberStatus.Active;
    
    public DateTime JoinedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}