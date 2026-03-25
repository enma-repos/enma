using Enma.Admin.Application.Models;

namespace Enma.Admin.Application.Dto.Users;

internal static class UserDtoMapper
{
    internal static UserDto ToDto(this User model)
        => new(
            Id: model.Id,
            Email: model.Email,
            DisplayName: model.DisplayName,
            AvatarUrl: model.AvatarUrl,
            Locale: model.Locale,
            Timezone: model.Timezone,
            CreatedAt: model.CreatedAt,
            UpdatedAt: model.UpdatedAt,
            DeletedAt: model.DeletedAt);
}

