using System.Text.Json.Nodes;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class ProjectEntity
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }
    public OrganizationEntity? Organization { get; set; }
    
    public required string Name { get; set; } 
    public required string Key { get; set; } 
    public string? Description { get; set; }
    public bool IsStared { get; set; }

    public Guid CreatedByUserId { get; set; }
    public UserEntity? CreatedByUser { get; set; }

    public JsonObject? Settings { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? ArchivedAt { get; set; }
    
    public ICollection<ProjectMemberEntity> Members { get; set; } = new List<ProjectMemberEntity>();
    public ICollection<SdkClientEntity> SdkClients { get; set; } = new List<SdkClientEntity>();
    public ICollection<ProcessDefinitionEntity> ProcessDefinitions { get; set; } = new List<ProcessDefinitionEntity>();
    public ICollection<EventDefinitionEntity> EventDefinitions { get; set; } = new List<EventDefinitionEntity>();
}