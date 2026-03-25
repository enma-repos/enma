namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class UserEntity
{
    /// <summary>
    /// Equals auth AccountId (shared id across services).
    /// </summary>
    public Guid Id { get; set; }

    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    
    public string? AvatarUrl { get; set; }
    public string? Locale { get; set; }
    public string? Timezone { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<OrganizationMemberEntity> OrganizationMemberships { get; set; } = new List<OrganizationMemberEntity>();
    public ICollection<ProjectMemberEntity> ProjectMemberships { get; set; } = new List<ProjectMemberEntity>();
    public ICollection<OrganizationEntity> OwnedOrganizations { get; set; } = new List<OrganizationEntity>();
}