using System.Text.Json;
using Enma.Common.Models;

namespace Enma.Ingest.Api.Dto;

public sealed record IngestEventDto
{
    public required Guid EventId { get; init; }
    public required Guid SdkClientId { get; init; }
    public required string EventName { get; init; } = string.Empty;

    public JsonElement? Payload { get; init; }
    public Dictionary<string, string>? Tags { get; init; } = [];

    public required List<ProcessKey> ProcessKeys { get; init; } = [];
    public required Actor Actor { get; init; }
    public required DateTime OccurredAt { get; init; }
}