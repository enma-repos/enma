using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface ISummaryService
{
    Task<Result<SummaryDto>> GetSummaryAsync(
        Guid organizationId,
        Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange,
        CancellationToken ct = default);
}
