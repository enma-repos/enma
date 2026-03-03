using Enma.Common.Models;
using FluentResults;

namespace Enma.Ingest.Application.Contracts.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task<Result> PublishAsync(EventMessageDto message, CancellationToken ct = default);
    Task<Result> PublishBatchAsync(List<EventMessageDto> messages, CancellationToken ct = default);
}
