using System.Diagnostics;
using Enma.BucketBuilder.Application.Abstractions;
using Enma.BucketBuilder.Application.ValueObjects;
using Enma.BucketBuilder.JobsOrchestration.Abstractions;
using Enma.BucketBuilder.JobsOrchestration.Contracts;
using Enma.BucketBuilder.JobsOrchestration.Models;
using Enma.BucketBuilder.JobsOrchestration.Options;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enma.BucketBuilder.JobsOrchestration.Services;

internal sealed class BucketBuildingPipeline : IBucketBuildingPipeline
{
    private static readonly TimeSpan BucketSize = TimeSpan.FromMinutes(5);

    private readonly IWindowBucketBuildService _windowBucketBuildService;
    private readonly IShardCheckpointRepository _checkpointRepository;
    private readonly BucketBuilderOptions _options;
    private readonly ILogger<BucketBuildingPipeline> _logger;

    public BucketBuildingPipeline(
        IWindowBucketBuildService windowBucketBuildService,
        IShardCheckpointRepository checkpointRepository,
        IOptions<BucketBuilderOptions> options,
        ILogger<BucketBuildingPipeline> logger)
    {
        ArgumentNullException.ThrowIfNull(windowBucketBuildService);
        ArgumentNullException.ThrowIfNull(checkpointRepository);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _windowBucketBuildService = windowBucketBuildService;
        _checkpointRepository = checkpointRepository;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ShardRunResult?> RunTickAsync(ShardDescriptor shard, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var pipeline = PipelineName.Rehydrate(_options.Pipeline);
        var ownerId = LeaseOwnerId.Rehydrate($"{Environment.MachineName}:{Environment.ProcessId}");
        var leaseTimeout = TimeSpan.FromSeconds(_options.LeaseTimeoutSeconds);

        var leaseResult = await _checkpointRepository.TryAcquireLeaseAsync(pipeline, shard, ownerId, leaseTimeout, ct);
        if (leaseResult.IsFailed)
        {
            _logger.LogError("Failed to acquire lease for shard {ShardIndex}/{ShardCount}: {Errors}",
                shard.Index, shard.Count, string.Join("; ", leaseResult.Errors.Select(e => e.Message)));
            return null;
        }

        var lease = leaseResult.Value;
        if (lease is null)
        {
            _logger.LogDebug("Shard {ShardIndex}/{ShardCount} is leased by another worker, skipping.", shard.Index, shard.Count);
            return null;
        }

        try
        {
            var checkpointResult = await _checkpointRepository.LoadAsync(pipeline, shard, ct);
            if (checkpointResult.IsFailed)
            {
                _logger.LogError("Failed to load checkpoint for shard {ShardIndex}/{ShardCount}: {Errors}",
                    shard.Index, shard.Count, string.Join("; ", checkpointResult.Errors.Select(e => e.Message)));
                return null;
            }

            var checkpoint = checkpointResult.Value;
            var now = DateTime.UtcNow;
            var safetyLag = TimeSpan.FromSeconds(_options.SafetyLagSeconds);
            var ceiling = Floor5Min(now - safetyLag);

            var startFrom = checkpoint?.LastCompletedBucketEndUtc?.Value
                ?? Floor5Min(now - TimeSpan.FromHours(_options.InitialLookbackHours));

            var windows = ComputePendingWindows(startFrom, ceiling, _options.MaxWindowsPerTick);

            if (windows.Count == 0)
            {
                _logger.LogDebug("Shard {ShardIndex}: no pending windows (ceiling={Ceiling}).", shard.Index, ceiling);
                return ShardRunResult.Rehydrate(shard, 0, startFrom, startFrom, 0, sw.ElapsedMilliseconds);
            }

            _logger.LogInformation(
                "Shard {ShardIndex}: processing {WindowCount} windows [{From} - {To}]",
                shard.Index, windows.Count, windows[0].StartUtc, windows[^1].EndUtc);

            long totalEvents = 0;
            var processedWindows = 0;

            foreach (var window in windows)
            {
                var result = await _windowBucketBuildService.BuildWindowAsync(window, ct);
                if (result.IsFailed)
                {
                    _logger.LogError(
                        "Failed to build window {WindowStart}: {Errors}",
                        window.StartUtc, string.Join("; ", result.Errors.Select(e => e.Message)));
                    break;
                }

                totalEvents += result.Value.EventsRead;
                processedWindows++;

                var updatedCheckpoint = ShardCheckpoint.Rehydrate(
                    pipeline.Value,
                    shard,
                    window.EndUtc.Value,
                    ownerId.Value,
                    lease.ExpiresAtUtc,
                    DateTime.UtcNow);

                var saveResult = await _checkpointRepository.SaveAndRenewLeaseAsync(updatedCheckpoint, ownerId, leaseTimeout, ct);
                if (saveResult.IsFailed)
                {
                    _logger.LogError(
                        "Failed to save checkpoint for window {WindowStart}: {Errors}",
                        window.StartUtc, string.Join("; ", saveResult.Errors.Select(e => e.Message)));
                    break;
                }
            }

            sw.Stop();

            var lastEndUtc = processedWindows > 0
                ? windows[processedWindows - 1].EndUtc.Value
                : startFrom;

            return ShardRunResult.Rehydrate(
                shard,
                processedWindows,
                startFrom,
                lastEndUtc,
                totalEvents,
                sw.ElapsedMilliseconds);
        }
        finally
        {
            try
            {
                await _checkpointRepository.ReleaseLeaseAsync(pipeline, shard, ownerId, ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to release lease for shard {ShardIndex}/{ShardCount}.", shard.Index, shard.Count);
            }
        }
    }

    internal static DateTime Floor5Min(DateTime dt)
    {
        var ticks = dt.Ticks - (dt.Ticks % (BucketSize.Ticks));
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    internal static List<BucketWindow> ComputePendingWindows(DateTime startFrom, DateTime ceiling, int maxWindows)
    {
        var windows = new List<BucketWindow>();
        var current = startFrom;

        while (current + BucketSize <= ceiling && windows.Count < maxWindows)
        {
            var window = BucketWindow.Rehydrate(current, current + BucketSize);
            windows.Add(window);
            current += BucketSize;
        }

        return windows;
    }
}
