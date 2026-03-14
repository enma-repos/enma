using Enma.Admin.Application.Dto.EventDefinitions;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IEventDefinitionsService
{
    Task<Result<EventDefinitionDto>> CreateAsync(CreateEventDefinitionDto dto, CancellationToken ct = default);
    Task<Result<EventDefinitionDto>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<EventDefinitionDto>> GetByProjectAndNameAsync(Guid projectId, string name, CancellationToken ct = default);

    Task<Result<IReadOnlyList<EventDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        int offset,
        int limit,
        CancellationToken ct = default);

    Task<Result> SetDescriptionAsync(Guid id, SetEventDefinitionDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct = default);
}
