using System.Text.Json.Nodes;

namespace Enma.Admin.Application.Dto.AuditLogs;

public sealed record CreateAuditLogDto(
    Guid OrganizationId,
    Guid? ProjectId,
    Guid? ActorUserId,
    string? Action,
    string? ResourceType,
    string? ResourceId,
    string? Ip,
    JsonObject? Payload);

