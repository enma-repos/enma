using Enma.Admin.Application.Dto.Super;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

// Future: SoftDeleteAsync, RestoreAsync, TransferOwnershipAsync.
public interface ISuperOrganizationsService
{
    Task<Result<PaginatedResult<SuperOrganizationListItemDto>>> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        CancellationToken ct = default);

    Task<Result<SuperOrganizationDetailsDto>> GetByIdAsync(Guid organizationId, CancellationToken ct = default);
}
