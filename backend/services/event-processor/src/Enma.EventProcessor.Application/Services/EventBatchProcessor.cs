using Enma.Common.Models;
using Enma.EventProcessor.Application.Abstractions;
using Enma.EventProcessor.Application.Contracts.Persistence.ClickHouse;
using Microsoft.Extensions.Logging;

namespace Enma.EventProcessor.Application.Services;

internal sealed class EventBatchProcessor : IEventBatchProcessor
{
    private readonly IEventsRepository _eventsRepository;
    private readonly ILogger<EventBatchProcessor> _logger;

    public EventBatchProcessor(
        IEventsRepository eventsRepository,
        ILogger<EventBatchProcessor> logger)
    {
        _eventsRepository = eventsRepository;
        _logger = logger;
    }

    public async Task ProcessBatchAsync(IReadOnlyCollection<EventMessageDto> events, CancellationToken ct = default)
    {
        if (events.Count == 0)
        {
            return;
        }
 
        // TODO: add validation
        
        _logger.LogInformation("Processing batch of {Count} events.", events.Count);
        await _eventsRepository.InsertBatchAsync(events, ct);
        _logger.LogInformation("Inserted batch of {Count} events into ClickHouse.", events.Count);
    }
}
