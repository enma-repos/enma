using System.Text.Json.Nodes;

namespace Enma.Admin.Application.Dto.AuditLogs;

public sealed record AuditLogDto(
    Guid Id,
    Guid OrganizationId,
    Guid? ProjectId,
    Guid? ActorUserId,
    string Action,
    string ResourceType,
    string ResourceId,
    string? Ip,
    JsonObject? Payload,
    DateTime CreatedAt);

