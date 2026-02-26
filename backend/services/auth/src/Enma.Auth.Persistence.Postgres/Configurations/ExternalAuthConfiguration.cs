using Enma.Auth.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Auth.Persistence.Postgres.Configurations;

internal sealed class ExternalAuthConfiguration : IEntityTypeConfiguration<ExternalAuthEntity>
{
    public void Configure(EntityTypeBuilder<ExternalAuthEntity> builder)
    {
        builder.ToTable("external_auth");
        builder.HasKey(x => new { x.Provider, x.Subject });

        builder.Property(x => x.Provider)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Subject)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.LinkedAt)
            .IsRequired();
        
        builder.HasOne(x => x.Account)
            .WithMany(a => a.ExternalAuths)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}