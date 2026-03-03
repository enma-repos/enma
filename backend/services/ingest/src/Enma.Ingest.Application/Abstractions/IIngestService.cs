using Enma.Common.Models;
using FluentResults;

namespace Enma.Ingest.Application.Abstractions;

public interface IIngestService
{
    Task<Result> IngestBatchAsync(List<EventMessageDto> events, CancellationToken ct = default);
}
