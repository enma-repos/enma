using Enma.Auth.Application.Dto.AdminUsers;
using FluentResults;

namespace Enma.Auth.Application.Contracts.Infrastructure.Grpc.Admin;

public interface IAdminUsersClient
{
    Task<Result<AdminUserDto>> CreateUserAsync(CreateAdminUserDto dto, CancellationToken ct = default);
    Task<Result<AdminUserDto>> GetUserAsync(Guid accountId, CancellationToken ct = default);
    Task<Result<bool>> ExistsUserAsync(Guid accountId, CancellationToken ct = default);
}
