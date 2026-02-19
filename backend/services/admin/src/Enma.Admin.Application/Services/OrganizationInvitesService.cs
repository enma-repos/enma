using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationInvitesService : IOrganizationInvitesService
{
    private readonly IOrganizationInvitesRepository _organizationInvitesRepository;

    public OrganizationInvitesService(IOrganizationInvitesRepository organizationInvitesRepository)
    {
        _organizationInvitesRepository = organizationInvitesRepository;
    }
}