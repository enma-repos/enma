namespace Enma.Common.Options;

public sealed record RabbitMqOptions
{
    public required string ConnectionString { get; init; }
}