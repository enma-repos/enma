using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Organizations;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationsService : IOrganizationsService
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IProjectsRepository _projectsRepository;

    public OrganizationsService(
        IOrganizationsRepository organizationsRepository,
        IProjectsRepository projectsRepository)
    {
        _organizationsRepository = organizationsRepository;
        _projectsRepository = projectsRepository;
    }

    public async Task<Result<OrganizationDto>> CreateAsync(
        CreateOrganizationDto dto,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var modelRes = Organization.Create(
            id: Guid.NewGuid(),
            name: dto.Name,
            description: dto.Description,
            slug: dto.Slug,
            ownerUserId: dto.CreatedByUserId,
            createdByUserId: dto.CreatedByUserId,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<OrganizationDto>(modelRes.Errors);
        }

        var res = await _organizationsRepository.CreateAsync(modelRes.Value, ct);
        if (res.IsFailed)
        {
            return Result.Fail<OrganizationDto>(res.Errors);
        }

        var defaultProjectRes = Project.Create(
            id: Guid.NewGuid(),
            orgId: res.Value.Id,
            name: "default",
            key: "default",
            description: null,
            isStared: false,
            createdByUserId: dto.CreatedByUserId,
            settings: null,
            createdAt: now);

        if (defaultProjectRes.IsSuccess)
        {
            await _projectsRepository.CreateAsync(defaultProjectRes.Value, ct);
        }

        return Result.Ok(res.Value.ToDto());
    }

    public async Task<Result<OrganizationDto>> GetByIdAsync(
        Guid orgId, 
        CancellationToken ct = default)
    {
        var res = await _organizationsRepository.GetByIdAsync(orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationDto>(res.Errors);
    }

    public async Task<Result<OrganizationDto>> GetBySlugAsync(
        string slug, 
        CancellationToken ct = default)
    {
        var res = await _organizationsRepository.GetBySlugAsync(slug, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<OrganizationDto>>> ListByUserAsync(
        Guid userId, 
        int offset, 
        int limit, 
        CancellationToken ct = default)
    {
        var res = await _organizationsRepository.ListByUserAsync(userId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<OrganizationDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<OrganizationDto>>(res.Errors);
    }

    public async Task<Result> SetNameAsync(
        Guid orgId, 
        SetOrganizationNameDto dto, 
        CancellationToken ct = default)
    {
        var res = await _organizationsRepository.SetNameAsync(orgId, dto.Name, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetOwnerAsync(
        Guid orgId, 
        SetOrganizationOwnerDto dto, 
        CancellationToken ct = default)
    {
        var res = await _organizationsRepository.SetOwnerAsync(orgId, dto.OwnerUserId, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SoftDeleteAsync(Guid orgId, Guid userId, CancellationToken ct = default)
    {
        var orgsRes = await _organizationsRepository.ListByUserAsync(userId, 0, 2, ct);
        if (orgsRes.IsSuccess && orgsRes.Value.Count <= 1)
        {
            return Result.Fail("Cannot delete the last organization.");
        }

        return await _organizationsRepository.SoftDeleteAsync(orgId, ct);
    }
}
