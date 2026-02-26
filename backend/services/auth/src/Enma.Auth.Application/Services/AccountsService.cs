using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Dto.Responses;
using Enma.Auth.Application.Models;
using FluentResults;

namespace Enma.Auth.Application.Services;

internal sealed class AccountsService : IAccountsService
{
    private readonly IAccountsRepository _accountsRepository;

    public AccountsService(IAccountsRepository accountsRepository)
    {
        _accountsRepository = accountsRepository;
    }

    public async Task<Result<GetAccountResponseDto>> GetByIdAsync(Guid accountId, CancellationToken ct = default)
    {
        var result = await _accountsRepository.GetByIdAsync(accountId, ct);
        return result.IsSuccess
            ? Result.Ok(ToDto(result.Value))
            : Result.Fail<GetAccountResponseDto>(result.Errors);
    }

    public async Task<Result<GetAccountResponseDto>> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var result = await _accountsRepository.GetByEmailAsync(email, ct);
        return result.IsSuccess
            ? Result.Ok(ToDto(result.Value))
            : Result.Fail<GetAccountResponseDto>(result.Errors);
    }

    public Task<Result> UpdateStatusAsync(Guid accountId, Enma.Common.Enums.AccountStatus status, CancellationToken ct = default)
        => _accountsRepository.UpdateStatusAsync(accountId, status, ct);

    private static GetAccountResponseDto ToDto(Account account)
        => new(
            Id: account.Id,
            Email: account.Email,
            Status: account.Status,
            LastLoginAt: account.LastLoginAt,
            OnboardingStartedAt: account.OnboardingStartedAt,
            OnboardingCompletedAt: account.OnboardingCompletedAt,
            CreatedAt: account.CreatedAt,
            UpdatedAt: account.UpdatedAt);
}
