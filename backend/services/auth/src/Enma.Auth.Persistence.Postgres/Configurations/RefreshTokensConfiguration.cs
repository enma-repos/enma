using Enma.Auth.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Auth.Persistence.Postgres.Configurations;

internal sealed class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TokenHash)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.LastUsedAt)
            .IsRequired();
        
        builder.HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Индексы
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.ExpiresAt);
    }
}