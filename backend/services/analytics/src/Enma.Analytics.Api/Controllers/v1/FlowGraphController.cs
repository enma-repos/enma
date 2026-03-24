using Enma.Analytics.Application.Abstractions;
using Enma.Analytics.Application.Dto;
using Enma.Analytics.Application.ValueObjects;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Analytics.Api.Controllers.v1;

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}/process-definitions/{processDefinitionId:guid}")]
[ApiController]
public sealed class FlowGraphController(IFlowGraphService service) : ControllerBase
{
    [HttpGet("flow")]
    public async Task<IActionResult> GetFlowAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromRoute] Guid processDefinitionId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] string? entryEvent,
        CancellationToken ct)
    {
        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var filter = new ProcessFilter(organizationId, projectId, processDefinitionId, dateRange.Value);

        var res = string.IsNullOrWhiteSpace(entryEvent)
            ? await service.GetFlowGraphAsync(filter, ct)
            : await service.GetFlowGraphByEntryEventAsync(filter, entryEvent, ct);

        return res.ToActionResult();
    }
}

[Route("api/analytics/v1/organizations/{organizationId:guid}/projects/{projectId:guid}")]
[ApiController]
public sealed class MultiProcessFlowGraphController(IFlowGraphService service) : ControllerBase
{
    [HttpPost("flow")]
    public async Task<IActionResult> GetMultiProcessFlowAsync(
        [FromRoute] Guid organizationId,
        [FromRoute] Guid projectId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromBody] MultiProcessFlowRequest request,
        CancellationToken ct)
    {
        if (request.ProcessDefinitionIds.Count == 0)
            return BadRequest("At least one processDefinitionId is required.");

        var dateRange = DateRange.Create(from, to);
        if (dateRange.IsFailed) return dateRange.ToActionResult();

        var filter = new MultiProcessFilter(organizationId, projectId, request.ProcessDefinitionIds, dateRange.Value);
        var res = await service.GetMultiProcessFlowGraphAsync(filter, ct);
        return res.ToActionResult();
    }
}
