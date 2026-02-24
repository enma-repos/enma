using Enma.Auth.Application.Contracts.Infrastructure.ExternalAuth;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Abstractions;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Http;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Options;
using Enma.Auth.Infrastructure.ExternalAuth.Google.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Infrastructure.ExternalAuth.Google.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleProviderOptions>(configuration.GetSection("GoogleProvider"));
        services.AddSingleton<IExternalIdentityProvider, GoogleExternalIdentityProvider>();
        services.AddSingleton<IGoogleIdTokenValidator, GoogleIdTokenValidator>();

        services.AddHttpClient<GoogleAuthClient>((sp, http) =>
        {
            var options = sp.GetRequiredService<IOptions<GoogleProviderOptions>>().Value;

            http.BaseAddress = new Uri(options.BaseAddress);
            http.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });
        
        return services;
    }
}