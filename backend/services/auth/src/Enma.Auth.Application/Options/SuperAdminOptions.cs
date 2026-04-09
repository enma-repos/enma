namespace Enma.Auth.Application.Options;

public sealed record SuperAdminOptions
{
    // Single comma-separated list, e.g. "alice@example.com,bob@example.com".
    // Bound from "SuperAdmin:EmailsRaw" configuration.
    public string EmailsRaw { get; init; } = string.Empty;
}
