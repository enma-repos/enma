using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/v1/organizations/{organizationId}/projects/{projectId}/members")]
[ApiController]
public sealed class ProjectMembersController(IProjectMembersService projectMembersService) : ControllerBase
{
    
}