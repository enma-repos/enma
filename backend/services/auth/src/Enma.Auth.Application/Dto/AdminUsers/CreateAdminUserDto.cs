namespace Enma.Auth.Application.Dto.AdminUsers;

public sealed record CreateAdminUserDto(
    Guid AccountId,
    string DisplayName,
    string? AvatarUrl,
    string? Locale,
    string? Timezone);

