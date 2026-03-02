namespace Enma.Common.Models;

public readonly record struct ProcessKey
{
    public required Guid ProcessDefinitionId { get; init; } // создается в админке (название процесса, тип (ProcessType), описание). К какому процессу принадлежит этот ивент
    public required string ProcessId { get; init; } // Вхождение процесса (например, сессия или orderId)
}