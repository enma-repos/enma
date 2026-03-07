using Enma.Common.Models;

namespace Enma.EventProcessor.Application.Abstractions;

public interface IEventBatchProcessor
{
    Task ProcessBatchAsync(IReadOnlyCollection<EventMessageDto> events, CancellationToken ct = default);
}
