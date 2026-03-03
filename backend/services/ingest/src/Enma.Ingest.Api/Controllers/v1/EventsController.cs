using System.Text.Json;
using System.Text.Json.Serialization;
using Enma.Api.Shared.Extensions;
using Enma.Common.Models;
using Enma.Ingest.Api.Dto;
using Enma.Ingest.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Ingest.Api.Controllers.v1;

[Route("/api/ingest/v1/organizations/{organizationId}/projects/{projectId}/events")]
[ApiController]
public sealed class EventsController(IIngestService ingestService) : ControllerBase
{
    [HttpPost("batch")]
    public async Task<IActionResult> AddEventsBatchAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromBody] List<IngestEventDto> events,
        CancellationToken ct)
    {
        var batch = events.Select(e => new EventMessageDto
        {
            EventId = e.EventId,
            OrganizationId = organizationId,
            ProjectId = projectId,
            SdkClientId = e.SdkClientId,
            EventName = e.EventName,
            Payload = e.Payload,
            Tags = e.Tags,
            ProcessKeys = e.ProcessKeys,
            Actor = e.Actor,
            OccurredAt = e.OccurredAt
        }).ToList();

        var result = await ingestService.IngestBatchAsync(batch, ct);
        return result.ToActionResult();
    }
}
