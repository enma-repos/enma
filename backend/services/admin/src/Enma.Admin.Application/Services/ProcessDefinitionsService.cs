using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.ProcessDefinitions;
using Enma.Admin.Application.Models;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class ProcessDefinitionsService : IProcessDefinitionsService
{
    private readonly IProcessDefinitionsRepository _repository;

    public ProcessDefinitionsService(IProcessDefinitionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ProcessDefinitionDto>> CreateAsync(
        CreateProcessDefinitionDto dto,
        Guid orgId,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var modelRes = ProcessDefinition.Create(
            id: Guid.NewGuid(),
            projectId: dto.ProjectId,
            name: dto.Name,
            key: dto.Key,
            type: dto.Type,
            description: dto.Description,
            createdByUserId: dto.CreatedByUserId,
            createdAt: now);

        if (modelRes.IsFailed)
        {
            return Result.Fail<ProcessDefinitionDto>(modelRes.Errors);
        }

        var res = await _repository.CreateAsync(modelRes.Value, orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProcessDefinitionDto>(res.Errors);
    }

    public async Task<Result<ProcessDefinitionDto>> GetByIdAsync(
        Guid id,
        Guid projectId,
        Guid orgId,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByIdAsync(id, projectId, orgId, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProcessDefinitionDto>(res.Errors);
    }

    public async Task<Result<ProcessDefinitionDto>> GetByProjectAndKeyAsync(
        Guid projectId,
        Guid orgId,
        string key,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByProjectAndKeyAsync(projectId, orgId, key, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProcessDefinitionDto>(res.Errors);
    }

    public async Task<Result<PaginatedResult<ProcessDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default)
    {
        var countRes = await _repository.CountByProjectAsync(projectId, orgId, search, ct);
        if (countRes.IsFailed)
            return Result.Fail<PaginatedResult<ProcessDefinitionDto>>(countRes.Errors);

        var res = await _repository.ListByProjectAsync(projectId, orgId, page, pageSize, search, ct);
        if (res.IsFailed)
            return Result.Fail<PaginatedResult<ProcessDefinitionDto>>(res.Errors);

        var items = res.Value.Select(x => x.ToDto()).ToList();
        return PaginatedResult<ProcessDefinitionDto>.Create(items, countRes.Value, page, pageSize);
    }

    public async Task<Result> SetNameAsync(
        Guid id,
        Guid projectId,
        Guid orgId,
        SetProcessDefinitionNameDto dto,
        CancellationToken ct = default)
    {
        var res = await _repository.SetNameAsync(id, projectId, orgId, dto.Name, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetDescriptionAsync(
        Guid id,
        Guid projectId,
        Guid orgId,
        SetProcessDefinitionDescriptionDto dto,
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
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
