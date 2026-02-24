using Enma.Auth.Application.Contracts.Infrastructure.ExternalAuth;

namespace Enma.Auth.Application.Abstractions;

public interface IExternalAuthProviderFabric
{
    IExternalIdentityProvider Get(string provider);
    bool TryGet(string provider, out IExternalIdentityProvider? result);
}