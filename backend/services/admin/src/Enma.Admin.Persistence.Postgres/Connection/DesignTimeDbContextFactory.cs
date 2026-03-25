using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enma.Admin.Persistence.Postgres.Connection;

/// <summary>
/// Used by dotnet-ef CLI to create migrations at design time.
/// </summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgresDbContext>
{
    public PostgresDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<PostgresDbContext>();
        builder.UseNpgsql("Host=localhost;Database=enma_admin_design;Username=postgres;Password=postgres");
        return new PostgresDbContext(builder.Options);
    }
}
