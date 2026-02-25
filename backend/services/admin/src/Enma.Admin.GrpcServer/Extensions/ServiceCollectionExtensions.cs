using Microsoft.Extensions.DependencyInjection;

namespace Enma.Admin.GrpcServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminGrpcServer(this IServiceCollection services)
    {
        services.AddGrpc();
        return services;
    }
}

