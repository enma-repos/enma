using Enma.Auth.Application.Contracts.Infrastructure.Grpc.Admin;
using Enma.Auth.Application.Dto.AdminUsers;
using Enma.Auth.Infrastructure.Grpc.Admin.Mapping;
using Enma.Auth.Infrastructure.Grpc.Admin.Options;
using Enma.Auth.Infrastructure.Grpc.Admin.Utils;
using Enma.Common.Errors;
using Enma.Grpc.Admin.Users.V1;
using FluentResults;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace Enma.Auth.Infrastructure.Grpc.Admin.Clients;

internal sealed class AdminUsersClient : IAdminUsersClient
{
    private readonly AdminUsersService.AdminUsersServiceClient _client;
    private readonly AdminGrpcOptions _options;

    public AdminUsersClient(
        AdminUsersService.AdminUsersServiceClient client,
        IOptions<AdminGrpcOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<Result<AdminUserDto>> CreateUserAsync(CreateAdminUserDto dto, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.CreateUserAsync(
                    dto.ToProto(),
                    deadline: AdminGrpcUtils.GetDeadline(_options.DeadlineMs),
                    cancellationToken: ct)
                .ResponseAsync;

            if (response.User is null)
            {
                return Result.Fail<AdminUserDto>(ApplicationErrors.InvariantViolation("Admin returned empty user."));
            }

            return response.User.ToDtoResult();
        }
        catch (RpcException ex)
        {
            return Result.Fail<AdminUserDto>(AdminGrpcUtils.Map(ex, "User", $"accountId={dto.AccountId}"));
        }
    }

    public async Task<Result<AdminUserDto>> GetUserAsync(Guid accountId, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.GetUserAsync(
                    new GetUserRequest { AccountId = accountId.ToString() },
                    deadline: AdminGrpcUtils.GetDeadline(_options.DeadlineMs),
                    cancellationToken: ct)
                .ResponseAsync;

            if (response.User is null)
            {
                return Result.Fail<AdminUserDto>(ApplicationErrors.InvariantViolation("Admin returned empty user."));
            }

            return response.User.ToDtoResult();
        }
        catch (RpcException ex)
        {
            return Result.Fail<AdminUserDto>(AdminGrpcUtils.Map(ex, "User", $"accountId={accountId}"));
        }
    }

    public async Task<Result<bool>> ExistsUserAsync(Guid accountId, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.ExistsUserAsync(
                    new ExistsUserRequest { AccountId = accountId.ToString() },
                    deadline: AdminGrpcUtils.GetDeadline(_options.DeadlineMs),
                    cancellationToken: ct)
                .ResponseAsync;

            return Result.Ok(response.Exists);
        }
        catch (RpcException ex)
        {
            return Result.Fail<bool>(AdminGrpcUtils.Map(ex, "User", $"accountId={accountId}"));
        }
    }
}
