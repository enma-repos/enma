using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class AuditLogsConfiguration : IEntityTypeConfiguration<AuditLogEntity>
{
    public void Configure(EntityTypeBuilder<AuditLogEntity> builder)
    {
        builder.ToTable("audit_logs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Action)
            .HasMaxLength(128)
            .IsRequired();
        
        builder.Property(x => x.ResourceType)
            .HasMaxLength(64)
            .IsRequired();
        
        builder.Property(x => x.ResourceId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.Payload).HasColumnType("jsonb");

        builder.HasIndex(x => new { x.OrganizationId, x.CreatedAt });
        builder.HasIndex(x => new { x.ProjectId, x.CreatedAt });

        builder.HasOne(x => x.Organization)
           .WithMany()
           .HasForeignKey(x => x.OrganizationId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Project)
           .WithMany()
           .HasForeignKey(x => x.ProjectId)
           .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ActorUser)
            .WithMany()
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}