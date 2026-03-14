using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Services;
using Enma.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.Admin.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<SecurityOptions>()
            .Bind(configuration.GetSection("Security"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKeyPepper),
                "Security:ApiKeyPepper is required.")
            .ValidateOnStart();
        
        services.AddScoped<IApiKeysService, ApiKeysService>();
        services.AddScoped<IAuditLogsService, AuditLogsService>();
        services.AddScoped<IOrganizationInvitesService, OrganizationInvitesService>();
        services.AddScoped<IOrganizationMembersService, OrganizationMembersService>();
        services.AddScoped<IOrganizationsService, OrganizationsService>();
        services.AddScoped<IProjectMembersService, ProjectMembersService>();
        services.AddScoped<IEventDefinitionsService, EventDefinitionsService>();
        services.AddScoped<IProcessDefinitionsService, ProcessDefinitionsService>();
        services.AddScoped<IProjectsService, ProjectsService>();
        services.AddScoped<ISdkClientsService, SdkClientsService>();
        services.AddScoped<IUsersService, UsersService>();
        
        services.AddSingleton<ISecretService>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<SecurityOptions>>().Value;

            if (string.IsNullOrWhiteSpace(opt.ApiKeyPepper))
            {
                throw new InvalidOperationException("Security:ApiKeyPepper is not configured.");
            }

            return new SecretService(opt.ApiKeyPepper, prefixLength: 12);
        });
        
        return services;
    }
}