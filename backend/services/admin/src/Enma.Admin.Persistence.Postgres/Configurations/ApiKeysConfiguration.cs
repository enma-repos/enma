using Enma.Admin.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enma.Admin.Persistence.Postgres.Configurations;

internal sealed class ApiKeysConfiguration : IEntityTypeConfiguration<ApiKeyEntity>
{
    public void Configure(EntityTypeBuilder<ApiKeyEntity> builder)
    {
        builder.ToTable("api_keys");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.KeyPrefix)
            .HasMaxLength(32)
            .IsRequired();
        
        builder.Property(x => x.KeyHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => x.SdkClientId);
        builder.HasIndex(x => x.KeyHash).IsUnique();

        builder.HasOne(x => x.SdkClient)
            .WithMany(c => c.Keys)
            .HasForeignKey(x => x.SdkClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}