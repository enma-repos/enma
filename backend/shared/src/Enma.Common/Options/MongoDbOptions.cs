namespace Enma.Common.Options;

public sealed record MongoDbOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = "enma";
}
