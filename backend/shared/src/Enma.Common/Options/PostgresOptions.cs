namespace Enma.Common.Options;

public sealed record PostgresOptions
{
    public required string ConnectionString { get; init; }
}