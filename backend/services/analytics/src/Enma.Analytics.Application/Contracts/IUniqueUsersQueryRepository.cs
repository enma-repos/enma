using Enma.Analytics.Application.Models;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Contracts;

public interface IUniqueUsersQueryRepository
{
    Task<Result<UniqueUsersCount>> GetUniqueUsersAsync(
        Guid organizationId,
        Guid projectId,
        IReadOnlyList<Guid>? processDefinitionIds,
        DateRange dateRange,
        CancellationToken ct = default);
}
