using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Super;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services.Super;

internal sealed class SuperOrganizationsService : ISuperOrganizationsService
{
    private const int MaxPageSize = 200;

    private readonly IOrganizationsRepository _organizationsRepository;

    public SuperOrganizationsService(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }

    public async Task<Result<PaginatedResult<SuperOrganizationListItemDto>>> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var listRes = await _organizationsRepository.ListSuperAsync(page, pageSize, search, includeDeleted, ct);
        if (listRes.IsFailed) return Result.Fail<PaginatedResult<SuperOrganizationListItemDto>>(listRes.Errors);

        var countRes = await _organizationsRepository.CountSuperAsync(search, includeDeleted, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<SuperOrganizationListItemDto>>(countRes.Errors);

        return PaginatedResult<SuperOrganizationListItemDto>.Create(listRes.Value, countRes.Value, page, pageSize);
    }

    public async Task<Result<SuperOrganizationDetailsDto>> GetByIdAsync(Guid organizationId, CancellationToken ct = default)
    {
        return await _organizationsRepository.GetSuperDetailsAsync(organizationId, ct);
    }
}
