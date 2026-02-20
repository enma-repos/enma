using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.Organizations;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationsService : IOrganizationsService
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public OrganizationsService(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
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
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<OrganizationDto>(res.Errors);
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

    public Task<Result> SoftDeleteAsync(Guid orgId, CancellationToken ct = default)
        => _organizationsRepository.SoftDeleteAsync(orgId, ct);
}
