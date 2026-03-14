using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface IEntryExitPointsService
{
    Task<Result<EntryExitPointsDto>> GetEntryExitPointsAsync(
        ProcessFilter filter, int limit, CancellationToken ct = default);
}
