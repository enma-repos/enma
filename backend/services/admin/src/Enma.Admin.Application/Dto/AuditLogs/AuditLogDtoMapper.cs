using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.AuditLogs;

internal static class AuditLogDtoMapper
{
    internal static AuditLogDto ToDto(this AuditLog model)
        => new(
            Id: model.Id,
            OrganizationId: model.OrganizationId,
            ProjectId: model.ProjectId,
            ActorUserId: model.ActorUserId,
            Action: model.Action,
            ResourceType: model.ResourceType,
            ResourceId: model.ResourceId,
            Ip: model.Ip?.ToString(),
            Payload: model.Payload,
            CreatedAt: model.CreatedAt);
}

