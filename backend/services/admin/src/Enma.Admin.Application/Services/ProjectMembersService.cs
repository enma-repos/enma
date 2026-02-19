using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class ProjectMembersService : IProjectMembersService
{
    private readonly IProjectMembersRepository _projectMembersRepository;

    public ProjectMembersService(IProjectMembersRepository projectMembersRepository)
    {
        _projectMembersRepository = projectMembersRepository;
    }
}