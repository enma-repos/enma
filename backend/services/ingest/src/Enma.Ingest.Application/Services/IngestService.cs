using Enma.Ingest.Application.Abstractions;
using Enma.Common.Models;
using Enma.Ingest.Application.Contracts.Infrastructure.Messaging;
using FluentResults;

namespace Enma.Ingest.Application.Services;

internal sealed class IngestService : IIngestService
{
    private readonly IEventPublisher _eventPublisher;
    public IngestService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public async Task<Result> IngestBatchAsync(
        List<EventMessageDto> events, 
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        
        foreach (var @event in events)
        {
            @event.IngestedAt = now;
        }
        
        return await _eventPublisher.PublishBatchAsync(events, ct);
    }
}
