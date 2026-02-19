using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly PostgresDbContext _context;

    public UsersRepository(PostgresDbContext context)
    {
        _context = context;
    }
}