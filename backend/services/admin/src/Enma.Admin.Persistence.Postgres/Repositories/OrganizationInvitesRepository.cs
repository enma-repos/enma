using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class OrganizationInvitesRepository : IOrganizationInvitesRepository
{
    private readonly PostgresDbContext _context;

    public OrganizationInvitesRepository(PostgresDbContext context)
    {
        _context = context;
    }
}