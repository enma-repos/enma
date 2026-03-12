namespace Enma.BucketBuilder.JobsOrchestration.Options;

public sealed record BucketBuilderOptions
{
    public int PollIntervalSeconds { get; init; } = 10;
    public int SafetyLagSeconds { get; init; } = 30;
    public int MaxWindowsPerTick { get; init; } = 12;
    public int LeaseTimeoutSeconds { get; init; } = 120;
    public string Pipeline { get; init; } = "bucket-builder";
    public int ShardCount { get; init; } = 1;
    public int InitialLookbackHours { get; init; } = 1;
}
