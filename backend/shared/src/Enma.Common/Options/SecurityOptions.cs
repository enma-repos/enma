namespace Enma.Common.Options;

public sealed record SecurityOptions
{
    public string ApiKeyPepper { get; init; } = string.Empty;
}