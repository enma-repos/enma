using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class EventDefinitionsConfiguration : IEntityTypeConfiguration<EventDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<EventDefinitionEntity> builder)
    {
        builder.ToTable("event_definitions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(512);

        builder.HasIndex(x => new { x.ProjectId, x.Name }).IsUnique();

        builder.HasOne(x => x.Project)
            .WithMany(p => p.EventDefinitions)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
