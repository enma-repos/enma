using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/v1/organizations/{organizationId}/invites")]
[ApiController]
public sealed class OrganizationInvitesController(IOrganizationInvitesService organizationInvitesService) 
    : ControllerBase
{
    
}