using Enma.Admin.Application.Dto.Super;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

// Future: SoftDeleteAsync, RestoreAsync, ArchiveAsync, UnarchiveAsync.
public interface ISuperProjectsService
{
    Task<Result<PaginatedResult<SuperProjectListItemDto>>> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        Guid? organizationId,
        CancellationToken ct = default);

    Task<Result<SuperProjectDetailsDto>> GetByIdAsync(Guid projectId, CancellationToken ct = default);
}
