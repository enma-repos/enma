using Enma.Auth.Application.Dto.Responses;
using Enma.Auth.Application.Models;

namespace Enma.Auth.Application.Mapping;

internal static class AccountMapping
{
    public static GetAccountResponseDto ToDto(this Account account)
        => new(
            Id: account.Id,
            Email: account.Email.Value,
            Status: account.Status,
            LastLoginAt: account.LastLoginAt,
            OnboardingStartedAt: account.OnboardingStartedAt,
            OnboardingCompletedAt: account.OnboardingCompletedAt,
            CreatedAt: account.CreatedAt,
            UpdatedAt: account.UpdatedAt);
}