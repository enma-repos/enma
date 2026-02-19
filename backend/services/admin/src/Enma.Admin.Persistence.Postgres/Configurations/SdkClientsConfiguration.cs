using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class SdkClientsConfiguration : IEntityTypeConfiguration<SdkClientEntity>
{
    public void Configure(EntityTypeBuilder<SdkClientEntity> builder)
    {
        builder.ToTable("sdk_clients");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(512);
        
        builder.Property(x => x.Settings).HasColumnType("jsonb");

        builder.HasIndex(x => x.ProjectId);

        builder.HasOne(x => x.Project)
            .WithMany(p => p.SdkClients)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}