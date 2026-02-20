namespace Enma.Admin.Application.Dto.Users;

public sealed record UserDto(
    Guid Id,
    string DisplayName,
    string? AvatarUrl,
    string? Locale,
    string? Timezone,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt);

