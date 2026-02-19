using System.Net;
using System.Text.Json.Nodes;

namespace Enma.Admin.Persistence.Postgres.Entities;

internal sealed class AuditLogEntity
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }
    public OrganizationEntity? Organization { get; set; }

    public Guid? ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    public Guid? ActorUserId { get; set; }
    public UserEntity? ActorUser { get; set; }
    
    public required string Action { get; set; }
    public required string ResourceType { get; set; } 
    public required string ResourceId { get; set; }
    
    public IPAddress? Ip { get; set; }
    public JsonObject? Payload { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}