using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;
using Enma.Admin.Application.Dto.EventDefinitions;
using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Services;

internal sealed class EventDefinitionsService : IEventDefinitionsService
{
    private readonly IEventDefinitionsRepository _repository;

    public EventDefinitionsService(IEventDefinitionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<EventDefinitionDto>> CreateAsync(
        CreateEventDefinitionDto dto,
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

        var res = await _repository.CreateAsync(modelRes.Value, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<EventDefinitionDto>(res.Errors);
    }

    public async Task<Result<EventDefinitionDto>> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByIdAsync(id, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<EventDefinitionDto>(res.Errors);
    }

    public async Task<Result<EventDefinitionDto>> GetByProjectAndNameAsync(
        Guid projectId,
        string name,
        CancellationToken ct = default)
    {
        var res = await _repository.GetByProjectAndNameAsync(projectId, name, ct);
        return res.IsSuccess
            ? Result.Ok(res.Value.ToDto())
            : Result.Fail<EventDefinitionDto>(res.Errors);
    }

    public async Task<Result<IReadOnlyList<EventDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default)
    {
        var res = await _repository.ListByProjectAsync(projectId, offset, limit, ct);
        return res.IsSuccess
            ? Result.Ok<IReadOnlyList<EventDefinitionDto>>(res.Value.Select(x => x.ToDto()).ToList())
            : Result.Fail<IReadOnlyList<EventDefinitionDto>>(res.Errors);
    }

    public async Task<Result> SetDescriptionAsync(
        Guid id,
        SetEventDefinitionDescriptionDto dto,
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
