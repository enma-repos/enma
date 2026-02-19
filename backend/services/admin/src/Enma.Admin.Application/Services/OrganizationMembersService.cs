using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class OrganizationMembersService : IOrganizationMembersService
{
    private readonly IOrganizationMembersRepository _organizationMembersRepository;

    public OrganizationMembersService(IOrganizationMembersRepository organizationMembersRepository)
    {
        _organizationMembersRepository = organizationMembersRepository;
    }
}