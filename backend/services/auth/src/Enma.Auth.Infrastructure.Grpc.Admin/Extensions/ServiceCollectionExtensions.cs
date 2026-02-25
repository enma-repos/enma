using Enma.Auth.Infrastructure.Grpc.Admin.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enma.Auth.Infrastructure.Grpc.Admin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdminGrpcOptions>(configuration.GetSection("AdminGrpc"));
        
        return services;
    }
}