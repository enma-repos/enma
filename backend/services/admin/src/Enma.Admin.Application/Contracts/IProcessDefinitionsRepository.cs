using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

public interface IProcessDefinitionsRepository
{
    Task<Result<ProcessDefinition>> CreateAsync(ProcessDefinition model, Guid orgId, CancellationToken ct = default);
    Task<Result<ProcessDefinition>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<ProcessDefinition>> GetByProjectAndKeyAsync(Guid projectId, Guid orgId, string key, CancellationToken ct = default);

    Task<Result<IReadOnlyList<ProcessDefinition>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetNameAsync(Guid id, Guid projectId, Guid orgId, string name, CancellationToken ct = default);
    Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, string? description, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
}
