using Enma.Admin.Application.Dto.ProcessDefinitions;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IProcessDefinitionsService
{
    Task<Result<ProcessDefinitionDto>> CreateAsync(CreateProcessDefinitionDto dto, CancellationToken ct = default);
    Task<Result<ProcessDefinitionDto>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<ProcessDefinitionDto>> GetByProjectAndKeyAsync(Guid projectId, string key, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProcessDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid id, SetProcessDefinitionNameDto dto, CancellationToken ct = default);
    Task<Result> SetDescriptionAsync(Guid id, SetProcessDefinitionDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
