using System.Reflection;
using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enma.Admin.Persistence.Postgres.Connection;

internal sealed class PostgresDbContext : DbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<OrganizationEntity> Organizations => Set<OrganizationEntity>();
    public DbSet<OrganizationMemberEntity> OrganizationMembers => Set<OrganizationMemberEntity>();
    public DbSet<OrganizationInviteEntity> OrganizationInvites => Set<OrganizationInviteEntity>();
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<ProjectMemberEntity> ProjectMembers => Set<ProjectMemberEntity>();
    public DbSet<SdkClientEntity> ApiClients => Set<SdkClientEntity>();
    public DbSet<ApiKeyEntity> ApiKeys => Set<ApiKeyEntity>();
    public DbSet<AuditLogEntity> AuditLogs => Set<AuditLogEntity>();
    
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(PostgresDbContext))!);
        base.OnModelCreating(modelBuilder);
    }
}