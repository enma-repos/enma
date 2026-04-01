using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.EventDefinitions;
using Enma.Admin.Application.Models;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class EventDefinitionsService : IEventDefinitionsService
{
    private readonly IEventDefinitionsRepository _repository;
    private readonly IEventDefinitionCacheInvalidator _cacheInvalidator;

    public EventDefinitionsService(
        IEventDefinitionsRepository repository,
        IEventDefinitionCacheInvalidator cacheInvalidator)
    {
        _repository = repository;
        _cacheInvalidator = cacheInvalidator;
    }

    public async Task<Result<EventDefinitionDto>> CreateAsync(
        CreateEventDefinitionDto dto,
        Guid orgId,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var modelRes = EventDefinition.Create(
            id: Guid.NewGuid(),
            projectId: dto.ProjectId,
            name: dto.Name,
            description: dto.Description,
            createdByUserId: dto.CreatedByUserId,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<EventDefinitionDto>(modelRes.Errors);
        }

        var res = await _repository.CreateAsync(modelRes.Value, orgId, ct);
        if (res.IsSuccess)
        {
            await _cacheInvalidator.InvalidateAsync(orgId, dto.ProjectId, ct);
        }

        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<EventDefinitionDto>(res.Errors);
    }

    public async Task<Result<EventDefinitionDto>> GetByIdAsync(
        Guid id,
        Guid projectId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByIdAsync(id, projectId, orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<EventDefinitionDto>(res.Errors);
    }

    public async Task<Result<EventDefinitionDto>> GetByProjectAndNameAsync(
        Guid projectId,
        Guid orgId,
        string name,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByProjectAndNameAsync(projectId, orgId, name, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<EventDefinitionDto>(res.Errors);
    }

    public async Task<Result<PaginatedResult<EventDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        var res = await _repository.ListByProjectAsync(projectId, orgId, page, pageSize, search, ct);
        if (res.IsFailed) return Result.Fail<PaginatedResult<EventDefinitionDto>>(res.Errors);

        var countRes = await _repository.CountByProjectAsync(projectId, orgId, search, ct);
        if (countRes.IsFailed) return Result.Fail<PaginatedResult<EventDefinitionDto>>(countRes.Errors);

        var items = res.Value.Select(x => x.ToDto()).ToList();
        return PaginatedResult<EventDefinitionDto>.Create(items, countRes.Value, page, pageSize);
    }

    public async Task<Result> SetDescriptionAsync(
        Guid id,
        Guid projectId,
        Guid orgId,
        SetEventDefinitionDescriptionDto dto,
        CancellationToken ct = default)
    {
        var res = await _repository.SetDescriptionAsync(id, projectId, orgId, dto.Description, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SoftDeleteAsync(
        Guid id,
        Guid projectId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _repository.SoftDeleteAsync(id, projectId, orgId, ct);
        if (res.IsSuccess)
        {
            await _cacheInvalidator.InvalidateAsync(orgId, projectId, ct);
        }

        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
