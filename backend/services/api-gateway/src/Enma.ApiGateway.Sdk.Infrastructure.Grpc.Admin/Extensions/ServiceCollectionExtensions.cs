using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Abstractions;
using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Clients;
using Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Options;
using Enma.Grpc.Admin.ApiKeys.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enma.ApiGateway.Sdk.Infrastructure.Grpc.Admin.Extensions;

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

        services.AddGrpcClient<AdminApiKeysService.AdminApiKeysServiceClient>((sp, o) =>
        {
            var opts = sp.GetRequiredService<IOptions<AdminGrpcOptions>>().Value;
            o.Address = new Uri(opts.Address);
        });

        services.AddScoped<IAdminApiKeysClient, AdminApiKeysClient>();

        return services;
    }
}
