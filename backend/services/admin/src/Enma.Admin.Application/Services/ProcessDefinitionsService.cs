using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.ProcessDefinitions;
using Enma.Admin.Application.Models;
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

        var res = await _repository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProcessDefinitionDto>(res.Errors);
    }

    public async Task<Result<ProcessDefinitionDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByIdAsync(id, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProcessDefinitionDto>(res.Errors);
    }

    public async Task<Result<ProcessDefinitionDto>> GetByProjectAndKeyAsync(
        Guid projectId,
        string key,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByProjectAndKeyAsync(projectId, key, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<ProcessDefinitionDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<ProcessDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var res = await _repository.ListByProjectAsync(projectId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<ProcessDefinitionDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<ProcessDefinitionDto>>(res.Errors);
    }

    public async Task<Result> SetNameAsync(
        Guid id,
        SetProcessDefinitionNameDto dto,
        CancellationToken ct = default)
    {
        var res = await _repository.SetNameAsync(id, dto.Name, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SetDescriptionAsync(
        Guid id,
        SetProcessDefinitionDescriptionDto dto,
        CancellationToken ct = default)
    {
        var res = await _repository.SetDescriptionAsync(id, dto.Description, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }

    public async Task<Result> SoftDeleteAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var res = await _repository.SoftDeleteAsync(id, ct);
        return res.IsSuccess
            ? Result.Ok()
            : Result.Fail(res.Errors);
    }
}
