using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Super;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services.Super;

internal sealed class SuperProjectsService : ISuperProjectsService
{
    private const int MaxPageSize = 200;

    private readonly IProjectsRepository _projectsRepository;

    public SuperProjectsService(IProjectsRepository projectsRepository)
    {
        _projectsRepository = projectsRepository;
    }

    public async Task<Result<PaginatedResult<SuperProjectListItemDto>>> ListAsync(
        int page,
        int pageSize,
        string? search,
        bool includeDeleted,
        Guid? organizationId,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        var listRes = await _projectsRepository.ListSuperAsync(page, pageSize, search, includeDeleted, organizationId, ct);
        if (listRes.IsFailed) return Result.Fail<PaginatedResult<SuperProjectListItemDto>>(listRes.Errors);

        var countRes = await _projectsRepository.CountSuperAsync(search, includeDeleted, organizationId, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<SuperProjectListItemDto>>(countRes.Errors);

        return PaginatedResult<SuperProjectListItemDto>.Create(listRes.Value, countRes.Value, page, pageSize);
    }

    public async Task<Result<SuperProjectDetailsDto>> GetByIdAsync(Guid projectId, CancellationToken ct = default)
    {
        return await _projectsRepository.GetSuperDetailsAsync(projectId, ct);
    }
}
