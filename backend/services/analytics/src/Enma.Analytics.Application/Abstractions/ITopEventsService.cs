using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface ITopEventsService
{
    Task<Result<TopEventsDto>> GetTopEventsAsync(
        ProcessFilter filter, string sortBy, int limit, CancellationToken ct = default);
}
