using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationsService : IOrganizationsService
{
    private readonly IOrganizationsRepository _organizationsRepository;

    public OrganizationsService(IOrganizationsRepository organizationsRepository)
    {
        _organizationsRepository = organizationsRepository;
    }
}