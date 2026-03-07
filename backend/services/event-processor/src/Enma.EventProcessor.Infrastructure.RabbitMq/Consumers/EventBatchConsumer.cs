using Enma.Common.Models;
using Enma.EventProcessor.Application.Abstractions;
using MassTransit;

namespace Enma.EventProcessor.Infrastructure.RabbitMq.Consumers;

internal sealed class EventBatchConsumer : IConsumer<Batch<EventMessageDto>>
{
    private readonly IEventBatchProcessor _eventBatchProcessor;

    public EventBatchConsumer(IEventBatchProcessor eventBatchProcessor)
    {
        _eventBatchProcessor = eventBatchProcessor;
    }

    public async Task Consume(ConsumeContext<Batch<EventMessageDto>> context)
    {
        var events = new EventMessageDto[context.Message.Length];

        for (var index = 0; index < context.Message.Length; index++)
        {
            events[index] = context.Message[index].Message;
        }

        await _eventBatchProcessor.ProcessBatchAsync(events, context.CancellationToken);
    }
}
