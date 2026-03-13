using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

public interface IProcessDefinitionsRepository
{
    Task<Result<ProcessDefinition>> CreateAsync(ProcessDefinition model, CancellationToken ct = default);
    Task<Result<ProcessDefinition>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<ProcessDefinition>> GetByProjectAndKeyAsync(Guid projectId, string key, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProcessDefinition>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid id, string name, CancellationToken ct = default);
    Task<Result> SetDescriptionAsync(Guid id, string? description, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
