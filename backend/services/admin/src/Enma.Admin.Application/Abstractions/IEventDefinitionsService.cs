using Enma.Admin.Application.Dto.EventDefinitions;
using Enma.Common.Models;
using FluentResults;

namespace Enma.Admin.Application.Abstractions;

public interface IEventDefinitionsService
{
    Task<Result<EventDefinitionDto>> CreateAsync(CreateEventDefinitionDto dto, Guid orgId, CancellationToken ct = default);
    Task<Result<EventDefinitionDto>> GetByIdAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
    Task<Result<EventDefinitionDto>> GetByProjectAndNameAsync(Guid projectId, Guid orgId, string name, CancellationToken ct = default);

    Task<Result<PaginatedResult<EventDefinitionDto>>> ListByProjectAsync(
        Guid projectId,
        Guid orgId,
        int page,
        int pageSize,
        string? search = null,
        CancellationToken ct = default);

    Task<Result> SetDescriptionAsync(Guid id, Guid projectId, Guid orgId, SetEventDefinitionDescriptionDto dto, CancellationToken ct = default);
    Task<Result> SoftDeleteAsync(Guid id, Guid projectId, Guid orgId, CancellationToken ct = default);
}
