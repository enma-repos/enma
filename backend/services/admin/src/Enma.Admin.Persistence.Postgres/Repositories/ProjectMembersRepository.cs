using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ProjectMembersRepository : IProjectMembersRepository
{
    private readonly PostgresDbContext _context;

    public ProjectMembersRepository(PostgresDbContext context)
    {
        _context = context;
    }
}