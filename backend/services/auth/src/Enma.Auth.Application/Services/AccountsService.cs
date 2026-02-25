using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Dto.Responses;
using Enma.Common.Enums;
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
        => throw new NotImplementedException();

    public async Task<Result<GetAccountResponseDto>> GetByEmailAsync(string email, CancellationToken ct = default)
        => throw new NotImplementedException();

    public async Task<Result> UpdateStatusAsync(Guid accountId, AccountStatus status, CancellationToken ct = default)
        => await _accountsRepository.UpdateStatusAsync(accountId, status, ct);
}
