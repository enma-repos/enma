using System.Net;
using System.Text.Json.Nodes;
using Enma.Common.Errors;
using FluentResults;

namespace Enma.Admin.Application.Models;

public sealed class AuditLog
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    
    public Guid? ProjectId { get; private set; }
    public Guid? ActorUserId { get; private set; }

    public string Action { get; private set; } = null!;
    public string ResourceType { get; private set; } = null!;
    public string ResourceId { get; private set; } = null!;
    
    public IPAddress? Ip { get; private set; }
    public JsonObject? Payload { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    private AuditLog() { }
    
    private AuditLog(Guid id, Guid orgId, Guid? projectId, Guid? actorUserId, string action, string resourceType,
        string resourceId, IPAddress? ip, JsonObject? payload, DateTime createdAt)
    {
        Id = id;
        OrganizationId = orgId;
        ProjectId = projectId;
        ActorUserId = actorUserId;
        Action = action;
        ResourceType = resourceType;
        ResourceId = resourceId;
        Ip = ip;
        Payload = payload;
        CreatedAt = createdAt;
    }
    
    public static Result<AuditLog> Create(Guid id, Guid orgId, Guid? projectId, Guid? actorUserId, string? action,
        string? resourceType, string? resourceId, IPAddress? ip, JsonObject? payload, DateTime createdAt)
    {
        if (orgId == Guid.Empty)
        {
            return Result.Fail<AuditLog>(ApplicationErrors.Required(nameof(orgId)));
        }

        action = (action ?? string.Empty).Trim();
        resourceType = (resourceType ?? string.Empty).Trim();
        resourceId = (resourceId ?? string.Empty).Trim();

        if (action.Length is < 3 or > 128)
        {
            return Result.Fail<AuditLog>(ApplicationErrors.Length(nameof(action), 3, 128));
        }

        if (resourceType.Length is < 2 or > 64)
        {
            return Result.Fail<AuditLog>(ApplicationErrors.Length(nameof(resourceType), 2, 64));
        }

        if (resourceId.Length is < 1 or > 128)
        {
            return Result.Fail<AuditLog>(ApplicationErrors.Length(nameof(resourceId), 1, 128));
        }

        return Result.Ok(new AuditLog(
            id, orgId, projectId, actorUserId,
            action, resourceType, resourceId,
            ip, payload, createdAt));
    }

    internal static AuditLog Rehydrate(Guid id, Guid orgId, Guid? projectId, Guid? actorUserId, string action,
        string resourceType, string resourceId, IPAddress? ip, JsonObject? payload, DateTime createdAt)
    {
        return new AuditLog
        {
            Id = id,
            OrganizationId = orgId,
            ProjectId = projectId,
            ActorUserId = actorUserId,
            Action = action,
            ResourceType = resourceType,
            ResourceId = resourceId,
            Ip = ip,
            Payload = payload,
            CreatedAt = createdAt
        };
    }
}