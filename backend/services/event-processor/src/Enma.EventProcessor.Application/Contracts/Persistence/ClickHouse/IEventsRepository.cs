using Enma.Common.Models;

namespace Enma.EventProcessor.Application.Contracts.Persistence.ClickHouse;

public interface IEventsRepository
{
    Task InsertBatchAsync(IReadOnlyCollection<EventMessageDto> events, CancellationToken ct = default);
}
