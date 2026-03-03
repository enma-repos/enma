using Enma.Common.Errors;
using Enma.Common.Models;
using Enma.Ingest.Application.Contracts.Infrastructure.Messaging;
using FluentResults;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Enma.Ingest.Infrastructure.RabbitMq.Services;

internal sealed class MassTransitEventPublisher : IEventPublisher
{
    // TODO: move chunk size to configuration
    private const int ChunkSize = 200;

    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MassTransitEventPublisher> _logger;

    public MassTransitEventPublisher(
        IPublishEndpoint publishEndpoint,
        ILogger<MassTransitEventPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Result> PublishAsync(
        EventMessageDto message,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(message.EventName))
        {
            return Result.Fail(ApplicationErrors.Required(nameof(message.EventName)));
        }

        await _publishEndpoint.Publish(message, ctx =>
        {
            ctx.MessageId = message.EventId;

            ctx.Headers.Set("enma.orgId", message.OrganizationId.ToString());
            ctx.Headers.Set("enma.projectId", message.ProjectId.ToString());
            ctx.Headers.Set("enma.sdkClientId", message.SdkClientId.ToString());
            ctx.Headers.Set("enma.eventName", message.EventName);
        }, ct);

        return Result.Ok();
    }

    public async Task<Result> PublishBatchAsync(
        List<EventMessageDto> messages,
        CancellationToken ct = default)
    {
        if (messages.Count == 0)
        {
            return Result.Ok();
        }

        if (messages.Any(m => string.IsNullOrWhiteSpace(m.EventName)))
        {
            return Result.Fail(ApplicationErrors.Required("EventName"));
        }

        _logger.LogInformation("Start batching {count} messages.", messages.Count);

        foreach (var chunk in messages.Chunk(ChunkSize))
        {
            await _publishEndpoint.PublishBatch(chunk, ct);
            _logger.LogInformation("Batched chunk of {count} messages.", chunk.Length);
        }
        
        _logger.LogInformation("Batching done.");
        return Result.Ok();
    }
}