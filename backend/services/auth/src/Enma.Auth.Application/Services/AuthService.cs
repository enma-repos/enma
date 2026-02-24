using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Perrsistence.Postgres;

namespace Enma.Auth.Application.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IExternalAuthProviderFabric _externalAuthProviderFabric;
    private readonly IAccountsRepository _accountsRepository;

    public AuthService(
        IExternalAuthProviderFabric externalAuthProviderFabric, 
        IAccountsRepository accountsRepository)
    {
        _externalAuthProviderFabric = externalAuthProviderFabric;
        _accountsRepository = accountsRepository;
    }
}