using System.Text.Json.Nodes;
using Enma.Common.Enums;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class SdkClientEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }
    
    public required string Name { get; set; }
    public SdkClientType Type { get; set; }
    public string? Description { get; set; }

    public JsonObject? Settings { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DisabledAt { get; set; }
    
    public ICollection<ApiKeyEntity> Keys { get; set; } = new List<ApiKeyEntity>();
}