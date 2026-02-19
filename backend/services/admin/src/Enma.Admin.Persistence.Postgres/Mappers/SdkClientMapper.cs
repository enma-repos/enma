using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class SdkClientMapper
{
    internal static SdkClient ToModel(this SdkClientEntity entity)
        => SdkClient.Rehydrate(
            id: entity.Id,
            projectId: entity.ProjectId,
            name: entity.Name,
            description: entity.Description,
            type: entity.Type,
            settings: entity.Settings,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            disabledAt: entity.DisabledAt);

    internal static SdkClientEntity ToEntity(this SdkClient model)
        => new()
        {
            Id = model.Id,
            ProjectId = model.ProjectId,
            Name = model.Name,
            Type = model.Type,
            Description = model.Description,
            Settings = model.Settings,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DisabledAt = model.DisabledAt
        };
}

