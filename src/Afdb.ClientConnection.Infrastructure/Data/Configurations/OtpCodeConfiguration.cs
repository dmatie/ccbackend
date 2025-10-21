using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCodeEntity>
{
    public void Configure(EntityTypeBuilder<OtpCodeEntity> builder)
    {
        builder.ToTable("OtpCodes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(e => e.Code);
        builder.HasIndex(e => e.Email);
    }
}
