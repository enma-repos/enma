using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;
using Enma.Admin.Persistence.Postgres.Repositories;
using Enma.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.Admin.Persistence.Postgres.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetSection("Postgres"));

        services.AddDbContext<PostgresDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IOptions<PostgresOptions>>().Value;
            options.UseNpgsql(config.ConnectionString);
        });
        
        services.AddScoped<IApiKeysRepository, ApiKeysRepository>();
        services.AddScoped<IAuditLogsRepository, AuditLogsRepository>();
        services.AddScoped<IOrganizationInvitesRepository, OrganizationInvitesRepository>();
        services.AddScoped<IOrganizationMembersRepository, OrganizationMembersRepository>();
        services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();
        services.AddScoped<IProjectMembersRepository, ProjectMembersRepository>();
        services.AddScoped<IProcessDefinitionsRepository, ProcessDefinitionsRepository>();
        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddScoped<ISdkClientsRepository, SdkClientsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}