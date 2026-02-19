namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class OrganizationEntity
{
    public Guid Id { get; set; }
    
    public Guid OwnerUserId { get; set; }
    public UserEntity? OwnerUser { get; set; }

    public Guid CreatedByUserId { get; set; }
    public UserEntity? CreatedByUser { get; set; }
    
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigations
    public ICollection<OrganizationMemberEntity> Members { get; set; } = new List<OrganizationMemberEntity>();
    public ICollection<OrganizationInviteEntity> Invites { get; set; } = new List<OrganizationInviteEntity>();
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}