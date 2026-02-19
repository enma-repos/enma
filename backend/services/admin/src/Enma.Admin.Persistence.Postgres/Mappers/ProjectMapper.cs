using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class ProjectMapper
{
    internal static Project ToModel(this ProjectEntity entity)
        => Project.Rehydrate(
            id: entity.Id,
            orgId: entity.OrganizationId,
            name: entity.Name,
            key: entity.Key,
            description: entity.Description,
            isStared: entity.IsStared,
            createdByUserId: entity.CreatedByUserId,
            settings: entity.Settings,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            archivedAt: entity.ArchivedAt,
            deletedAt: entity.DeletedAt);

    internal static ProjectEntity ToEntity(this Project model)
        => new()
        {
            Id = model.Id,
            OrganizationId = model.OrganizationId,
            Name = model.Name,
            Key = model.Key.Value,
            Description = model.Description,
            IsStared = model.IsStared,
            CreatedByUserId = model.CreatedByUserId,
            Settings = model.Settings,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt,
            ArchivedAt = model.ArchivedAt
        };
}

