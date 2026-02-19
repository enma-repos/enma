using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class AuditLogsRepository : IAuditLogsRepository
{
    private readonly PostgresDbContext _context;

    public AuditLogsRepository(PostgresDbContext context)
    {
        _context = context;
    }
}