using Enma.Ingest.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Ingest.Api.Controllers.v1;

[Route("/api/ingest/v1/organizations/{organizationId}/projects/{projectId}/events")]
[ApiController]
public sealed class EventsController(IIngestService ingestService) : ControllerBase
{
    [HttpPost("batch")]
    public async Task<IActionResult> AddEventsBatchAsync()
    {
        throw new NotImplementedException();
    }
}