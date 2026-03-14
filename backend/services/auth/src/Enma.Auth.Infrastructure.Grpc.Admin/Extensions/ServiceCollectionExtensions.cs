using Enma.Auth.Application.Contracts.Infrastructure.Grpc.Admin;
using Enma.Auth.Infrastructure.Grpc.Admin.Clients;
using Enma.Auth.Infrastructure.Grpc.Admin.Options;
using Enma.Grpc.Admin.Users.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Infrastructure.Grpc.Admin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AdminGrpcOptions>()
            .Bind(configuration.GetSection("AdminGrpc"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Address),
                "AdminGrpc:Address is required.")
            .Validate(o => o.DeadlineMs > 0,
                "AdminGrpc:DeadlineMs must be greater than 0.")
            .ValidateOnStart();

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        services.AddGrpcClient<AdminUsersService.AdminUsersServiceClient>((sp, o) =>
        {
            var opts = sp.GetRequiredService<IOptions<AdminGrpcOptions>>().Value;
            o.Address = new Uri(opts.Address);
        });

        services.AddScoped<IAdminUsersClient, AdminUsersClient>();
        
        return services;
    }
}
