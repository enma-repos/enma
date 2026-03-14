using Enma.Admin.Application.Models;
using FluentResults;

namespace Enma.Admin.Application.Contracts;

public interface IEventDefinitionsRepository
{
    Task<Result<EventDefinition>> CreateAsync(EventDefinition model, CancellationToken ct = default);
    Task<Result<EventDefinition>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<EventDefinition>> GetByProjectAndNameAsync(Guid projectId, string name, CancellationToken ct = default);

    Task<Result<IReadOnlyList<EventDefinition>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetDescriptionAsync(Guid id, string? description, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
