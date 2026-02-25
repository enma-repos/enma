using Enma.Auth.Application.Contracts.Infrastructure.Security;
using Enma.Auth.Infrastructure.Security.Options;
using Enma.Auth.Infrastructure.Security.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Auth.Infrastructure.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddSingleton<IAccessTokenProvider, JwtProvider>();

        return services;
    }
}