using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class NotificationsConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(1024)
            .IsRequired();

        builder.HasOne(x => x.RecipientUser)
            .WithMany()
            .HasForeignKey(x => x.RecipientUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.RecipientUserId, x.IsRead })
            .HasFilter("\"IsRead\" = false");

        builder.HasIndex(x => new { x.RecipientUserId, x.CreatedAt })
            .IsDescending(false, true);

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
