using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class OrganizationMembersRepository : IOrganizationMembersRepository
{
    private readonly PostgresDbContext _context;

    public OrganizationMembersRepository(PostgresDbContext context)
    {
        _context = context;
    }
}