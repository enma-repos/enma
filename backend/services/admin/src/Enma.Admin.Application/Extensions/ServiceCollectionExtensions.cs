using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Admin.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApiKeysService, ApiKeysService>();
        services.AddScoped<IAuditLogsService, AuditLogsService>();
        services.AddScoped<IOrganizationInvitesService, OrganizationInvitesService>();
        services.AddScoped<IOrganizationMembersService, OrganizationMembersService>();
        services.AddScoped<IOrganizationsService, OrganizationsService>();
        services.AddScoped<IProjectMembersService, ProjectMembersService>();
        services.AddScoped<IProjectsService, ProjectsService>();
        services.AddScoped<ISdkClientsService, SdkClientsService>();
        services.AddScoped<IUsersService, UsersService>();
        
        return services;
    }
}