using Enma.Auth.Application.Contracts.Persistence.Postgres;
using Enma.Auth.Persistence.Postgres.Connection;
using Enma.Auth.Persistence.Postgres.Repositories;
using Enma.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Persistence.Postgres.Extensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetSection("Postgres"));

        services.AddDbContext<PostgresDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IOptions<PostgresOptions>>().Value;
            options.UseNpgsql(config.ConnectionString);
        });
        
        services.AddScoped<IAccountsRepository, AccountsRepository>();
        services.AddScoped<IExternalAuthRepository, ExternalAuthRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        
        return services;
    }
}