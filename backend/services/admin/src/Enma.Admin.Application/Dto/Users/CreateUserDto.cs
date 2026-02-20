namespace Enma.Admin.Application.Dto.Users;

public sealed record CreateUserDto(
    Guid AccountId,
    string DisplayName,
    string? AvatarUrl,
    string? Locale,
    string? Timezone);

