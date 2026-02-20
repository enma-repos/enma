using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Users;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Result<UserDto>> CreateAsync(
        CreateUserDto dto, 
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var modelRes = User.Create(
            accountId: dto.AccountId,
            displayName: dto.DisplayName,
            avatarUrl: dto.AvatarUrl,
            locale: dto.Locale,
            timezone: dto.Timezone,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<UserDto>(modelRes.Errors);
        }

        var res = await _usersRepository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<UserDto>(res.Errors);
    }

    public async Task<Result<UserDto>> GetByIdAsync(
        Guid userId, 
        CancellationToken ct = default)
    {
        var res = await _usersRepository.GetByIdAsync(userId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<UserDto>(res.Errors);
    }

    public Task<Result<bool>> ExistsAsync(Guid userId, CancellationToken ct = default)
        => _usersRepository.ExistsAsync(userId, ct);

    public async Task<Result> SetDisplayNameAsync(
        Guid userId, 
        SetUserDisplayNameDto dto, 
        CancellationToken ct = default)
    {
        var res = await _usersRepository.SetDisplayNameAsync(userId, dto.DisplayName, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetAvatarUrlAsync(
        Guid userId, 
        SetUserAvatarUrlDto dto, 
        CancellationToken ct = default)
    {
        var res = await _usersRepository.SetAvatarUrlAsync(userId, dto.AvatarUrl, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetLocaleAsync(
        Guid userId, 
        SetUserLocaleDto dto, 
        CancellationToken ct = default)
    {
        var res = await _usersRepository.SetLocaleAsync(userId, dto.Locale, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetTimezoneAsync(
        Guid userId, 
        SetUserTimezoneDto dto, 
        CancellationToken ct = default)
    {
        var res = await _usersRepository.SetTimezoneAsync(userId, dto.Timezone, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public Task<Result> SoftDeleteAsync(Guid userId, CancellationToken ct = default)
        => _usersRepository.SoftDeleteAsync(userId, ct);
}
