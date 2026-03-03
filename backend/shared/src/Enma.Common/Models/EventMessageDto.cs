using System.Text.Json;

namespace Enma.Common.Models;

public sealed class EventMessageDto
{
    public Guid EventId { get; init; } // generate in sdk
    
    public Guid OrganizationId { get; init; }
    public Guid ProjectId { get; init; }
    public Guid SdkClientId { get; init; }
    
    public required string EventName { get; init; } = string.Empty; // unique + static
    
    // Data
    public JsonElement? Payload { get; init; }
    public IReadOnlyDictionary<string, string>? Tags { get; init; }
    
    // То к каким процессам принадлежит ивент + вхождения процессов
    public required List<ProcessKey> ProcessKeys { get; init; } = []; 
    
    // Кем инициирован ивент (+ связь с идентификатором до авторизации)
    public required Actor Actor { get; init; }
    
    public DateTime OccurredAt { get; init; }
    public DateTime IngestedAt { get; set; }
}