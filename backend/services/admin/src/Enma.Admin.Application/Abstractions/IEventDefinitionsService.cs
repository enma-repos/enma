using Enma.Admin.Application.Dto.EventDefinitions;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IEventDefinitionsService
{
    Task<Result<EventDefinitionDto>> CreateAsync(CreateEventDefinitionDto dto, Guid orgId, CancellationToken ct = default);
    Task<Result<EventDefinitionDto>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<EventDefinitionDto>> GetByProjectAndNameAsync(Guid projectId, Guid orgId, string name, CancellationToken ct = default);

    Task<Result<IReadOnlyList<EventDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, SetEventDefinitionDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
}
