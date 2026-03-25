using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

public interface IEventDefinitionsRepository
{
    Task<Result<EventDefinition>> CreateAsync(EventDefinition model, Guid orgId, CancellationToken ct = default);
    Task<Result<EventDefinition>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<EventDefinition>> GetByProjectAndNameAsync(Guid projectId, Guid orgId, string name, CancellationToken ct = default);

    Task<Result<IReadOnlyList<EventDefinition>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, string? description, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
}
