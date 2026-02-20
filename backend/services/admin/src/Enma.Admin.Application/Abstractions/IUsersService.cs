using Enma.Admin.Application.Dto.Users;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IUsersService
{
    Task<Result<UserDto>> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
    Task<Result<UserDto>> GetByIdAsync(Guid userId, CancellationToken ct = default);
    Task<Result<bool>> ExistsAsync(Guid userId, CancellationToken ct = default);

    Task<Result> SetDisplayNameAsync(Guid userId, SetUserDisplayNameDto dto, CancellationToken ct = default);
    Task<Result> SetAvatarUrlAsync(Guid userId, SetUserAvatarUrlDto dto, CancellationToken ct = default);
    Task<Result> SetLocaleAsync(Guid userId, SetUserLocaleDto dto, CancellationToken ct = default);
    Task<Result> SetTimezoneAsync(Guid userId, SetUserTimezoneDto dto, CancellationToken ct = default);

    Task<Result> SoftDeleteAsync(Guid userId, CancellationToken ct = default);
}
