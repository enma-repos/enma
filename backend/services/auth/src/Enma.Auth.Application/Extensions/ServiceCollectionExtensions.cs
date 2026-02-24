using Enma.Auth.Application.Abstractions;
using Enma.Auth.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Auth.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountsService, AccountsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IExternalAuthProviderFabric, ExternalAuthProviderFabric>();
        
        return services;
    }
}