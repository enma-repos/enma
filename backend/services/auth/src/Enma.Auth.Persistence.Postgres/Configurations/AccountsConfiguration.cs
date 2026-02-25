using Enma.Auth.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Auth.Persistence.Postgres.Configurations;

internal sealed class AccountsConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ToTable("accounts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasConversion<byte>()
            .IsRequired();
        
        builder.Property(x => x.PasswordHash)
            .HasMaxLength(512);

        builder.Property(x => x.Salt)
            .HasMaxLength(128);

        builder.Property(x => x.LastLoginAt);

        builder.Property(x => x.OnboardingStartedAt)
           .IsRequired();

        builder.Property(x => x.OnboardingCompletedAt);

        builder.Property(x => x.CreatedAt)
           .IsRequired();

        builder.Property(x => x.UpdatedAt)
           .IsRequired();

        builder.Property(x => x.DeletedAt);
        
        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");
        
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DeletedAt);
    }
}