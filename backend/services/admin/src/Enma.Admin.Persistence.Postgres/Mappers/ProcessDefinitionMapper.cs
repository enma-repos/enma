using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;
using Enma.Common.Enums;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class ProcessDefinitionMapper
{
    internal static ProcessDefinition ToModel(this ProcessDefinitionEntity entity)
        => ProcessDefinition.Rehydrate(
            id: entity.Id,
            projectId: entity.ProjectId,
            name: entity.Name,
            key: entity.Key,
            type: (ProcessType)entity.Type,
            description: entity.Description,
            createdByUserId: entity.CreatedByUserId,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            deletedAt: entity.DeletedAt);

    internal static ProcessDefinitionEntity ToEntity(this ProcessDefinition model)
        => new()
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Name = model.Name,
            Key = model.Key.Value,
            Type = (int)model.Type,
            Description = model.Description,
            CreatedByUserId = model.CreatedByUserId,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt
        };
}
