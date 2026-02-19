using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class OrganizationMapper
{
    internal static Organization ToModel(this OrganizationEntity entity)
        => Organization.Rehydrate(
            id: entity.Id,
            name: entity.Name,
            description: entity.Description,
            slug: entity.Slug,
            ownerUserId: entity.OwnerUserId,
            createdByUserId: entity.CreatedByUserId,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            deletedAt: entity.DeletedAt);

    internal static OrganizationEntity ToEntity(this Organization model)
        => new()
        {
            Id = model.Id,
            OwnerUserId = model.OwnerUserId,
            CreatedByUserId = model.CreatedByUserId,
            Name = model.Name,
            Slug = model.Slug.Value,
            Description = model.Description,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt
        };
}

