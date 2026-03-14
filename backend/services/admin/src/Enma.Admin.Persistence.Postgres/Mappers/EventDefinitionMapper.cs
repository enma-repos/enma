using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class EventDefinitionMapper
{
    internal static EventDefinition ToModel(this EventDefinitionEntity entity)
        => EventDefinition.Rehydrate(
            id: entity.Id,
            projectId: entity.ProjectId,
            name: entity.Name,
            description: entity.Description,
            createdByUserId: entity.CreatedByUserId,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            deletedAt: entity.DeletedAt);

    internal static EventDefinitionEntity ToEntity(this EventDefinition model)
        => new()
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Name = model.Name,
            Description = model.Description,
            CreatedByUserId = model.CreatedByUserId,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt
        };
}
