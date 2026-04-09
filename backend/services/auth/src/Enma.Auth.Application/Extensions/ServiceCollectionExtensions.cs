using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Options;
using Enma.Auth.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Auth.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AuthOptions>()
            .Bind(configuration.GetSection("Auth"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.RedirectBaseAddress),
                "Auth:RedirectBaseAddress is required.")
            .Validate(o => o.RefreshTokenLifetimeDays > 0,
                "Auth:RefreshTokenLifetimeDays must be greater than 0.")
            .Validate(o => o.StateCacheTtlMinutes > 0,
                "Auth:StateCacheTtlMinutes must be greater than 0.")
            .ValidateOnStart();

        services.Configure<SuperAdminOptions>(configuration.GetSection("SuperAdmin"));

        services.AddScoped<IAccountsService, AccountsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IExternalAuthProviderFabric, ExternalAuthProviderFabric>();
        services.AddSingleton<ISuperAdminPolicy, SuperAdminPolicy>();

        return services;
    }
}