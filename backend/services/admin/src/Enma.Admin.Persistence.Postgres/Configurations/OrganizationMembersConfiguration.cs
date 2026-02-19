using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class OrganizationMembersConfiguration : IEntityTypeConfiguration<OrganizationMemberEntity>
{
    public void Configure(EntityTypeBuilder<OrganizationMemberEntity> builder)
    {
        builder.ToTable("organization_members");
        builder.HasKey(x => new { x.OrganizationId, x.UserId });
        
        builder.Property(x => x.UpdatedAt);
        builder.Property(x => x.JoinedAt);

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Organization)
           .WithMany(o => o.Members)
           .HasForeignKey(x => x.OrganizationId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.OrganizationMemberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}