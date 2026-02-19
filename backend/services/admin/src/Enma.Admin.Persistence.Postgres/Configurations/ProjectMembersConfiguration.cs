using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class ProjectMembersConfiguration : IEntityTypeConfiguration<ProjectMemberEntity>
{
    public void Configure(EntityTypeBuilder<ProjectMemberEntity> builder)
    {
        builder.ToTable("project_members");
        builder.HasKey(x => new { x.ProjectId, x.UserId });

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Project)
           .WithMany(p => p.Members)
           .HasForeignKey(x => x.ProjectId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.ProjectMemberships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}