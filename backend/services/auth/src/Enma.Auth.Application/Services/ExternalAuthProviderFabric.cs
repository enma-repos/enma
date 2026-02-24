using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Contracts.Infrastructure.ExternalAuth;

namespace Enma.Auth.Application.Services;

internal sealed class ExternalAuthProviderFabric : IExternalAuthProviderFabric
{
    private readonly IReadOnlyDictionary<string, IExternalIdentityProvider> _map;
    
    public ExternalAuthProviderFabric(IEnumerable<IExternalIdentityProvider> providers)
    {
        var dict = new Dictionary<string, IExternalIdentityProvider>(StringComparer.OrdinalIgnoreCase);

        foreach (var p in providers)
        {
            if (string.IsNullOrWhiteSpace(p.Name))
            {
                throw new InvalidOperationException($"External provider has empty Name. Type: {p.GetType().FullName}");
            }

            if (dict.TryGetValue(p.Name, out var provider))
            {
                throw new InvalidOperationException($"Duplicate external provider Name='{p.Name}'. " +
                                                    $"Types: {provider.GetType().FullName} and {p.GetType().FullName}");
            }

            dict[p.Name] = p;
        }

        _map = dict;
    }

    
    public IExternalIdentityProvider Get(string provider)
    {
        if (TryGet(provider, out var p))
        {
            return p!;
        }

        var available = string.Join(", ", _map.Keys.OrderBy(x => x));
        throw new KeyNotFoundException($"Unknown external auth provider '{provider}'. Available: [{available}]");
    }

    public bool TryGet(string provider, out IExternalIdentityProvider? result)
    {
        result = null;
        return !string.IsNullOrWhiteSpace(provider) && _map.TryGetValue(provider, out result);
    }
}