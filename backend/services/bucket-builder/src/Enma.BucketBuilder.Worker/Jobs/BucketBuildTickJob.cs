using Enma.BucketBuilder.JobsOrchestration.Abstractions;
using Enma.BucketBuilder.JobsOrchestration.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Enma.BucketBuilder.Worker.Jobs;

[DisallowConcurrentExecution]
public sealed class BucketBuildTickJob : IJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BucketBuildTickJob> _logger;

    public BucketBuildTickJob(IServiceScopeFactory scopeFactory, ILogger<BucketBuildTickJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var shardIndex = context.MergedJobDataMap.GetInt("ShardIndex");
        var shardCount = context.MergedJobDataMap.GetInt("ShardCount");

        var shardResult = ShardDescriptor.Create(shardIndex, shardCount);
        if (shardResult.IsFailed)
        {
            _logger.LogError("Invalid shard descriptor: index={ShardIndex}, count={ShardCount}", shardIndex, shardCount);
            return;
        }

        var shard = shardResult.Value;

        _logger.LogDebug("Tick started for shard {ShardIndex}/{ShardCount}", shard.Index, shard.Count);

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var pipeline = scope.ServiceProvider.GetRequiredService<IBucketBuildingPipeline>();
            var result = await pipeline.RunTickAsync(shard, context.CancellationToken);

            if (result is not null)
            {
                _logger.LogInformation(
                    "Shard {ShardIndex}: processed {Windows} windows, {Events} events in {Duration}ms",
                    shard.Index, result.WindowsProcessed, result.TotalEventsRead, result.TotalDurationMs);
            }
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Shard {ShardIndex} tick cancelled (shutdown).", shard.Index);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Shard {ShardIndex} tick failed.", shard.Index);
        }
    }
}
