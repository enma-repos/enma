using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Contracts;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.Enums;
using Enma.Analytics.Application.ValueObjects;
using FluentResults;

namespace Enma.Analytics.Application.Services;

internal sealed class TimeTrendsService(
    IPathNodeQueryRepository nodeRepo) : ITimeTrendsService
{
    public async Task<Result<TimeTrendsDto>> GetTimeTrendsAsync(
        ProcessFilter filter, Granularity granularity, CancellationToken ct = default)
    {
        var seriesResult = await nodeRepo.GetNodeTimeSeriesAsync(filter, granularity, ct);
        if (seriesResult.IsFailed)
            return Result.Fail<TimeTrendsDto>(seriesResult.Errors);

        var points = seriesResult.Value
            .Select(s => new TimeTrendPointDto(s.BucketStart, s.TotalVisits, s.TotalEntries, s.TotalExits, s.TotalUniqueChains))
            .ToList();

        return Result.Ok(new TimeTrendsDto(points, granularity.ToString()));
    }
}
