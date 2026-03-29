using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface ITopEventsService
{
    Task<Result<TopEventsDto>> GetTopEventsAsync(
        ProcessFilter filter, string sortBy, int limit, CancellationToken ct = default);

    Task<Result<TopEventsDto>> GetTopEventsAsync(
        Guid organizationId, Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange, string sortBy, int limit,
        CancellationToken ct = default);
}
