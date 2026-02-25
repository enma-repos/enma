namespace Enma.Auth.Application.Dto.AdminUsers;

public sealed record AdminUserDto(
    Guid AccountId,
    string DisplayName,
    string? AvatarUrl,
    string? Locale,
    string? Timezone,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt);

