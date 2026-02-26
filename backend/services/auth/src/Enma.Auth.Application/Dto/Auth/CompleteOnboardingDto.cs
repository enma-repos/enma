namespace Enma.Auth.Application.Dto.Auth;

public sealed record CompleteOnboardingDto(
    string DisplayName,
    string? AvatarUrl,
    string? Locale,
    string? Timezone);

