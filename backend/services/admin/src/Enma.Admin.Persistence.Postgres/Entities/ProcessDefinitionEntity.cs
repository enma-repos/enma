namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class ProcessDefinitionEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    public required string Name { get; set; }
    public required string Key { get; set; }
    public int Type { get; set; }
    public string? Description { get; set; }

    public Guid CreatedByUserId { get; set; }
    public UserEntity? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
