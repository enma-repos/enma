using Enma.Admin.Application.Abstractions;
using Enma.Grpc.Admin.Users.V1;
using Grpc.Core;

namespace Enma.Admin.GrpcServer.Services;

public sealed class AdminUsersService : Grpc.Admin.Users.V1.AdminUsersService.AdminUsersServiceBase
{
    private readonly IUsersService _usersService;

    public AdminUsersService(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        
        return await base.CreateUser(request, context);
    }

    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        return await base.GetUser(request, context);
    }
}