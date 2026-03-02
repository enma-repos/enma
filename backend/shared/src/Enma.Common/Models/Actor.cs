namespace Enma.Common.Models;

public readonly record struct Actor
{
    public string? UserId { get; init; }
    public string? AnonymousId { get; init; }
}