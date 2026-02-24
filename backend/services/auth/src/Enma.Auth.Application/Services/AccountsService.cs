using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Perrsistence.Postgres;

namespace Enma.Auth.Application.Services;

internal sealed class AccountsService : IAccountsService
{
    private readonly IAccountsRepository _accountsRepository;

    public AccountsService(IAccountsRepository accountsRepository)
    {
        _accountsRepository = accountsRepository;
    }
}