using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyEntity>
{
    public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
    {
        builder.ToTable("Currencies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Symbol)
            .HasMaxLength(10);
    }
}
