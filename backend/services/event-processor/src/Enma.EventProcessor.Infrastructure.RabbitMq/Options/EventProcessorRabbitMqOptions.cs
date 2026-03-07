namespace Enma.EventProcessor.Infrastructure.RabbitMq.Options;

internal sealed record EventProcessorRabbitMqOptions
{
    public required string EndpointName { get; init; }
    public ushort PrefetchCount { get; init; } 
    public int BatchMessageLimit { get; init; } 
    public int BatchTimeLimitSeconds { get; init; } 
    public int BatchConcurrencyLimit { get; init; } 
    public int RetryCount { get; init; } 
    public int RetryIntervalSeconds { get; init; } 
}
