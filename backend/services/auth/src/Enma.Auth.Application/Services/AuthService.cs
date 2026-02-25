using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Application.Dto.External;
using Enma.Auth.Application.Dto.Responses;
using FluentResults;

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

    public async Task<Result<GetAccountResponseDto>> AuthenticateExternalAsync(ExternalAuthRequestDto request, CancellationToken ct = default)
        => throw new NotImplementedException();
}
