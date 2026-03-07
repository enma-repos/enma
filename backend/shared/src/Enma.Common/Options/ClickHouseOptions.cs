namespace Enma.Common.Options;

public sealed record ClickHouseOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public bool ApplyMigrationsOnStartup { get; init; } = true;
    public int CommandTimeoutSeconds { get; init; } = 30;
}
