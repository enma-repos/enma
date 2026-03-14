using Enma.Auth.Application.Contracts.Infrastructure.Security;
using Enma.Auth.Infrastructure.Security.Services;
using Enma.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Auth.Infrastructure.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Jwt"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.SecretKey),
                "Jwt:SecretKey is required.")
            .Validate(o => o.ExpiresMinutes > 0,
                "Jwt:ExpiresMinutes must be greater than 0.")
            .ValidateOnStart();
        services.AddSingleton<IAccessTokenProvider, JwtProvider>();
        services.AddSingleton<ICryptographyService, CryptographyService>();

        return services;
    }
}