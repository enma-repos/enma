using Enma.Admin.Application.Dto.Super;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

// Future: SoftDeleteAsync, BanAsync, RestoreAsync — mutation surface.
public interface ISuperUsersService
{
    Task<Result<PaginatedResult<SuperUserListItemDto>>> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        CancellationToken ct = default);

    Task<Result<SuperUserDetailsDto>> GetByIdAsync(Guid userId, CancellationToken ct = default);
}
