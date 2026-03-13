using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class ProcessDefinitionsConfiguration : IEntityTypeConfiguration<ProcessDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<ProcessDefinitionEntity> builder)
    {
        builder.ToTable("process_definitions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Key)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(512);

        builder.HasIndex(x => new { x.ProjectId, x.Key }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany(p => p.ProcessDefinitions)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
