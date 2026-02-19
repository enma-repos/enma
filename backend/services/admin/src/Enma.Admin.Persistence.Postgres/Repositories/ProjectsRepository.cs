using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ProjectsRepository : IProjectsRepository
{
    private readonly PostgresDbContext _context;

    public ProjectsRepository(PostgresDbContext context)
    {
        _context = context;
    }
}