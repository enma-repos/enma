using System.Text.Json;

namespace Enma.Common.Models;

public sealed record EventMessageDto
{
    public required Guid EventId { get; init; } // generate in sdk
    
    public required Guid OrganizationId { get; init; }
    public required Guid ProjectId { get; init; }
    public required Guid SdkClientId { get; init; }
    
    public required string EventName { get; init; } = string.Empty; // unique + static
    
    // Data
    public JsonElement Payload { get; init; }
    public IReadOnlyDictionary<string, string>? Tags { get; init; }
    
    // То к каким процессам принадлежит ивент + вхождения процессов
    public required List<ProcessKey> ProcessKeys { get; init; } = []; 
    
    // Кем инициирован ивент (+ связь с идентификатором до авторизации)
    public required Actor ActorId { get; init; }
    
    public required DateTime OccurredAt { get; init; }
    public required DateTime IngestedAt { get; init; }
}