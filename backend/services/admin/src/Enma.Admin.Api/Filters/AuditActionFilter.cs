using System.Net;
using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Dto.AuditLogs;
using Enma.Api.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Enma.Admin.Api.Filters;

/// <summary>
/// Runs after controller actions decorated with <see cref="AuditActionAttribute"/>.
/// Writes an audit-log entry when the action completes with a 2xx status code.
/// </summary>
internal sealed class AuditActionFilter : IAsyncActionFilter
{
    private static readonly HashSet<string> SkipRouteParams = new(StringComparer.OrdinalIgnoreCase)
    {
        "organizationId",
        "projectId",
    };

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();

        var attr = executedContext.ActionDescriptor.EndpointMetadata
            .OfType<AuditActionAttribute>()
            .FirstOrDefault();

        if (attr is null)
            return;

        // Only log on successful responses (2xx).
        if (executedContext.Result is ObjectResult { StatusCode: >= 200 and < 300 }
            or StatusCodeResult { StatusCode: >= 200 and < 300 }
            or ObjectResult { StatusCode: null }) // OkObjectResult defaults to null → 200
        {
            // noop — fall through to logging
        }
        else if (executedContext.Result is StatusCodeResult sc && sc.StatusCode is >= 200 and < 300)
        {
            // also ok
        }
        else
        {
            return;
        }

        var auditService = context.HttpContext.RequestServices.GetRequiredService<IAuditLogsService>();

        var routeValues = context.RouteData.Values;

        var orgId = TryParseGuid(routeValues, "organizationId");
        var projectId = TryParseGuid(routeValues, "projectId");

        // Determine resourceId.
        string resourceId;
        if (!string.IsNullOrEmpty(attr.ResourceIdParam)
            && routeValues.TryGetValue(attr.ResourceIdParam, out var explicitVal)
            && explicitVal is not null)
        {
            resourceId = explicitVal.ToString()!;
        }
        else
        {
            resourceId = ResolveResourceId(executedContext, routeValues) ?? string.Empty;
        }

        context.HttpContext.User.TryGetAccountId(out var accountId);

        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        // For create actions the resource id is often in the response body.
        if (string.IsNullOrEmpty(resourceId))
        {
            resourceId = ExtractIdFromResult(executedContext.Result) ?? "unknown";
        }

        // If orgId is empty (e.g. OrganizationsController.Create returns it in body), try to extract.
        if (orgId == Guid.Empty && attr.ResourceType == "Organization")
        {
            if (Guid.TryParse(resourceId, out var parsed))
                orgId = parsed;
        }

        var dto = new CreateAuditLogDto(
            OrganizationId: orgId,
            ProjectId: projectId == Guid.Empty ? null : projectId,
            ActorUserId: accountId == Guid.Empty ? null : accountId,
            Action: attr.Action,
            ResourceType: attr.ResourceType,
            ResourceId: resourceId,
            Ip: ip,
            Payload: null);

        // Fire-and-forget style — we don't want audit failures to break the main response.
        try
        {
            await auditService.AppendAsync(dto, context.HttpContext.RequestAborted);
        }
        catch
        {
            // Audit logging must never fail the request.
        }
    }

    private static Guid TryParseGuid(Microsoft.AspNetCore.Routing.RouteValueDictionary values, string key)
    {
        if (values.TryGetValue(key, out var val) && val is not null && Guid.TryParse(val.ToString(), out var guid))
            return guid;
        return Guid.Empty;
    }

    /// <summary>
    /// Tries to find the resource id from route values (first guid that isn't org/project).
    /// </summary>
    private static string? ResolveResourceId(ActionExecutedContext ctx, Microsoft.AspNetCore.Routing.RouteValueDictionary routeValues)
    {
        foreach (var kv in routeValues)
        {
            if (SkipRouteParams.Contains(kv.Key))
                continue;

            if (kv.Value is not null && Guid.TryParse(kv.Value.ToString(), out _))
                return kv.Value.ToString();
        }

        return null;
    }

    /// <summary>
    /// For POST/create responses that return the created entity, extracts its Id.
    /// </summary>
    private static string? ExtractIdFromResult(IActionResult? result)
    {
        if (result is ObjectResult { Value: not null } objResult)
        {
            var idProp = objResult.Value.GetType().GetProperty("Id")
                         ?? objResult.Value.GetType().GetProperty("id");
            if (idProp is not null)
            {
                var val = idProp.GetValue(objResult.Value);
                return val?.ToString();
            }
        }

        return null;
    }
}
