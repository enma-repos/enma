using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface IEventDetailService
{
    Task<Result<EventDetailDto>> GetEventDetailAsync(
        ProcessFilter filter, string eventName, CancellationToken ct = default);
}
