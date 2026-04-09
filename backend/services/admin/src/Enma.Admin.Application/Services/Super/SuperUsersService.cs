using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Super;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services.Super;

internal sealed class SuperUsersService : ISuperUsersService
{
    private const int MaxPageSize = 200;

    private readonly IUsersRepository _usersRepository;

    public SuperUsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Result<PaginatedResult<SuperUserListItemDto>>> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var listRes = await _usersRepository.ListSuperAsync(page, pageSize, search, includeDeleted, ct);
        if (listRes.IsFailed) return Result.Fail<PaginatedResult<SuperUserListItemDto>>(listRes.Errors);

        var countRes = await _usersRepository.CountSuperAsync(search, includeDeleted, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<SuperUserListItemDto>>(countRes.Errors);

        return PaginatedResult<SuperUserListItemDto>.Create(listRes.Value, countRes.Value, page, pageSize);
    }

    public async Task<Result<SuperUserDetailsDto>> GetByIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await _usersRepository.GetSuperDetailsAsync(userId, ct);
    }
}
