using Microsoft.AspNetCore.Mvc.Filters;

namespace Enma.Admin.Api.Filters;

/// <summary>
/// Marks a controller action for automatic audit logging.
/// The filter reads route values to determine organizationId, projectId and resourceId.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class AuditActionAttribute : ActionFilterAttribute
{
    public string Action { get; }
    public string ResourceType { get; }

    /// <summary>
    /// Name of the route parameter that contains the resource ID.
    /// Falls back to the first Guid route value that is not organizationId/projectId.
    /// </summary>
    public string? ResourceIdParam { get; set; }

    public AuditActionAttribute(string action, string resourceType)
    {
        Action = action;
        ResourceType = resourceType;
    }
}
