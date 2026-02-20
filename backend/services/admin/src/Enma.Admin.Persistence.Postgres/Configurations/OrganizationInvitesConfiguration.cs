using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class OrganizationInvitesConfiguration : IEntityTypeConfiguration<OrganizationInviteEntity>
{
    public void Configure(EntityTypeBuilder<OrganizationInviteEntity> builder)
    {
        builder.ToTable("organization_invites");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TargetEmail)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(x => x.TokenHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasOne(x => x.Organization)
           .WithMany(o => o.Invites)
           .HasForeignKey(x => x.OrganizationId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
           .WithMany()
           .HasForeignKey(x => x.CreatedByUserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AcceptedUser)
           .WithMany()
           .HasForeignKey(x => x.AcceptedUserId)
           .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => new { x.OrganizationId, x.TargetEmail })
            .IsUnique()
            .HasFilter("\"AcceptedAt\" IS NULL");
    }
}