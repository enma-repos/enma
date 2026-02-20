using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/v1/organizations/{organizationId}")]
[ApiController]
public sealed class OrganizationsController(IOrganizationsService organizationsService) : ControllerBase
{
    
}