using Enma.Auth.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enma.Auth.Persistence.Postgres.Connection;

internal sealed class PostgresDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<ExternalAuthEntity> ExternalAuth { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
}