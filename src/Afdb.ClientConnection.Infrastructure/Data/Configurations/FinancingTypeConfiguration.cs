using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class FinancingTypeConfiguration : IEntityTypeConfiguration<FinancingTypeEntity>
{
    public void Configure(EntityTypeBuilder<FinancingTypeEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.NameFr)
            .IsRequired()
            .HasMaxLength(100);


        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Index pour les recherches fréquentes
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.IsActive);


        builder.HasData(
            new FinancingTypeEntity
            {
                Id = Guid.Parse("27cfc0a1-682f-4e97-8d22-6416617f3706"),
                Name = "Private",
                NameFr ="Privé",
                Code = "PRIVATE",
                Description = "Financement d'origine privée",
                IsActive = true,
            },
            new FinancingTypeEntity
            {
                Id = Guid.Parse("a975c9e2-0cb2-4f83-876c-7078aaf66abd"),
                Name = "Public",
                NameFr ="Public",
                Code = "PUBLIC",
                Description = "Financement d'origine publique",
                IsActive = true,
            },
            new FinancingTypeEntity
            {
                Id = Guid.Parse("1aefe328-1746-457c-9949-cfbaa3390c67"),
                Name = "Other",
                NameFr ="Autre",
                Code = "OTHER",
                Description = "Autre type de financement",
                IsActive = true,
            }
        );
    }
}
