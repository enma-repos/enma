using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("/api/v1/")]
[ApiController]
internal sealed class OrganizationInvitesController(IOrganizationInvitesService organizationInvitesService) 
    : ControllerBase
{
    
}