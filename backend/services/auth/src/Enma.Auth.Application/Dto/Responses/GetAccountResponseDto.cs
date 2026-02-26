using Enma.Common.Enums;

namespace Enma.Auth.Application.Dto.Responses;

public sealed record GetAccountResponseDto(
    Guid Id,
    string Email,
    AccountStatus Status,
    DateTime LastLoginAt,
    DateTime OnboardingStartedAt,
    DateTime? OnboardingCompletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);
