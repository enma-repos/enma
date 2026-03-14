using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Connection;

internal sealed class PostgresDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<OrganizationEntity> Organizations { get; set; }
    public DbSet<OrganizationMemberEntity> OrganizationMembers { get; set; }
    public DbSet<OrganizationInviteEntity> OrganizationInvites { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectMemberEntity> ProjectMembers { get; set; }
    public DbSet<SdkClientEntity> ApiClients { get; set; }
    public DbSet<ApiKeyEntity> ApiKeys { get; set; }
    public DbSet<AuditLogEntity> AuditLogs { get; set; }
    public DbSet<ProcessDefinitionEntity> ProcessDefinitions { get; set; }
    public DbSet<EventDefinitionEntity> EventDefinitions { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);
}