using Enma.Admin.Application.Dto.ProcessDefinitions;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IProcessDefinitionsService
{
    Task<Result<ProcessDefinitionDto>> CreateAsync(CreateProcessDefinitionDto dto, Guid orgId, CancellationToken ct = default);
    Task<Result<ProcessDefinitionDto>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<ProcessDefinitionDto>> GetByProjectAndKeyAsync(Guid projectId, Guid orgId, string key, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProcessDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid id, Guid projectId, Guid orgId, SetProcessDefinitionNameDto dto, CancellationToken ct = default);
    Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, SetProcessDefinitionDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
}
