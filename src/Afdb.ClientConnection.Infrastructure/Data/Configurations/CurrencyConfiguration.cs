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

        builder.HasData(
            new CurrencyEntity
            {
                Id = Guid.Parse("9f66cbd8-a197-40a1-ad5e-6d060f20b400"),
                Name = "US Dollar",
                Code = "USD",
                Symbol = "$"
            },
            new CurrencyEntity
            {
                Id = Guid.Parse("e11e7997-21cf-4310-a4d2-568d20caab63"),
                Name = "Euro",
                Code = "EUR",
                Symbol = "€"
            },
            new CurrencyEntity
            {
                Id = Guid.Parse("03bb614c-f288-4924-b247-6671b00b94b9"),
                Name = "British Pound",
                Code = "GBP",
                Symbol = "£"
            },
            new CurrencyEntity
            {
                Id = Guid.Parse("51128130-44f4-4243-865d-9be6c530cf3b"),
                Name = "Japanese Yen",
                Code = "JPY",
                Symbol = "¥"
            },
            new CurrencyEntity
            {
                Id = Guid.Parse("6e087735-4cab-40fe-aa2e-1482b6ca9452"),
                Name = "Franc CFA",
                Code = "XOF",
                Symbol = "CFA"
            }
        );
    }
}
