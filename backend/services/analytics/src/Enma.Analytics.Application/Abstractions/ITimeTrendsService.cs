using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Abstractions;

public interface ITimeTrendsService
{
    Task<Result<TimeTrendsDto>> GetTimeTrendsAsync(
        ProcessFilter filter, Granularity granularity, CancellationToken ct = default);
}
