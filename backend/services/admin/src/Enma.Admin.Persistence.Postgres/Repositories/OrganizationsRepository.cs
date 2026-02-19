using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class OrganizationsRepository : IOrganizationsRepository
{
    private readonly PostgresDbContext _context;

    public OrganizationsRepository(PostgresDbContext context)
    {
        _context = context;
    }
}