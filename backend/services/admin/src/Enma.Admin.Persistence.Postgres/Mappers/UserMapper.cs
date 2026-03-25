using Enma.Admin.Application.Models;
using Enma.Admin.Persistence.Postgres.Entities;

namespace Enma.Admin.Persistence.Postgres.Mappers;

internal static class UserMapper
{
    internal static User ToModel(this UserEntity entity)
        => User.Rehydrate(
            id: entity.Id,
            email: entity.Email,
            displayName: entity.DisplayName,
            avatarUrl: entity.AvatarUrl,
            locale: entity.Locale,
            timezone: entity.Timezone,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt,
            deletedAt: entity.DeletedAt);

    internal static UserEntity ToEntity(this User model)
        => new()
        {
            Id = model.Id,
            Email = model.Email,
            DisplayName = model.DisplayName,
            AvatarUrl = model.AvatarUrl,
            Locale = model.Locale,
            Timezone = model.Timezone,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt
        };
}
