namespace Enma.Common.Models;

public record Actor
{
    public string? UserId { get; init; }
    public string? AnonymousId { get; init; }
}