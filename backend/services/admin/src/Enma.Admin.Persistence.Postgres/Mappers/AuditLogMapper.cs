using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class AuditLogMapper
{
    internal static AuditLog ToModel(this AuditLogEntity entity)
        => AuditLog.Rehydrate(
            id: entity.Id,
            orgId: entity.OrganizationId,
            projectId: entity.ProjectId,
            actorUserId: entity.ActorUserId,
            action: entity.Action,
            resourceType: entity.ResourceType,
            resourceId: entity.ResourceId,
            ip: entity.Ip,
            payload: entity.Payload,
            createdAt: entity.CreatedAt);

    internal static AuditLogEntity ToEntity(this AuditLog model)
        => new()
        {
            Id = model.Id,
            OrganizationId = model.OrganizationId,
            ProjectId = model.ProjectId,
            ActorUserId = model.ActorUserId,
            Action = model.Action,
            ResourceType = model.ResourceType,
            ResourceId = model.ResourceId,
            Ip = model.Ip,
            Payload = model.Payload,
            CreatedAt = model.CreatedAt
        };
}

