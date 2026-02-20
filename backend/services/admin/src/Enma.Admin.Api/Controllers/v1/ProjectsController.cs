using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/v1/organizations/{organizationId}/projects")]
[ApiController]
public sealed class ProjectsController(IProjectsService projectsService) : ControllerBase
{
    
}