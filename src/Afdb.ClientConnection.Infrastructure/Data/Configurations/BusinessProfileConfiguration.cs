using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfileEntity>
{
    public void Configure(EntityTypeBuilder<BusinessProfileEntity> builder)
    {
        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Code).IsRequired().HasMaxLength(10);
        builder.Property(bp => bp.Name).IsRequired().HasMaxLength(100);
        builder.Property(bp => bp.NameFr).IsRequired().HasMaxLength(100);
        builder.Property(bp => bp.Description).HasMaxLength(500);
        builder.Property(bp => bp.IsActive).HasDefaultValue(true);
        builder.HasIndex(bp => bp.Code).IsUnique();


        // --- Données initiales ---
        builder.HasData(
            new BusinessProfileEntity
            {
                Id = Guid.Parse("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                Name = "Executing Agency",
                NameFr = "Agence d'exécution",
                Code = "EA",
                Description = "Agency responsible for execution of projects",
                IsActive = true
            },
            new BusinessProfileEntity
            {
                Id = Guid.Parse("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                Name = "Borrower",
                NameFr = "Emprunteur",
                Code = "BO",
                Description = "Borrower",
                IsActive = true
            },
            new BusinessProfileEntity
            {
                Id = Guid.Parse("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                Name = "Guarantor",
                NameFr = "Garant",
                Code = "GU",
                Description = "Guarantor",
                IsActive = true
            }
        );
    }
}
