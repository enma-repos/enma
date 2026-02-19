using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class OrganizationsConfiguration : IEntityTypeConfiguration<OrganizationEntity>
{
    public void Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        builder.ToTable("organizations");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();
        
        builder.Property(x => x.Slug)
            .HasMaxLength(64)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(512);
        
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.HasOne(x => x.OwnerUser)
           .WithMany(u => u.OwnedOrganizations)
           .HasForeignKey(x => x.OwnerUserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
           .WithMany()
           .HasForeignKey(x => x.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}