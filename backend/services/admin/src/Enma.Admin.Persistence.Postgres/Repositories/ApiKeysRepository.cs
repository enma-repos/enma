using Enma.Admin.Application.Contracts;
using Enma.Admin.Persistence.Postgres.Connection;

namespace Enma.Admin.Persistence.Postgres.Repositories;

internal sealed class ApiKeysRepository : IApiKeysRepository
{
    private readonly PostgresDbContext _context;

    public ApiKeysRepository(PostgresDbContext context)
    {
        _context = context;
    }
}