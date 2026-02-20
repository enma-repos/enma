using Enma.Admin.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Admin.Api.Controllers.v1;

[Route("api/v1/organizations/{organizationId}/projects/{projectId}/sdk-clients")]
[ApiController]
public sealed class SdkClientsController(ISdkClientsService sdkClientsService) : ControllerBase
{
    
}