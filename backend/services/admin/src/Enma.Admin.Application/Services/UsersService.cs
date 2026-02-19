using Enma.Admin.Application.Abstractions;
using Enma.Admin.Application.Contracts;

namespace Enma.Admin.Application.Services;

internal sealed class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
}