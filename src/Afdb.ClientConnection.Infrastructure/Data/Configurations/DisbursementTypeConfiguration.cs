using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementTypeConfiguration : IEntityTypeConfiguration<DisbursementTypeEntity>
{
    public void Configure(EntityTypeBuilder<DisbursementTypeEntity> builder)
    {
        builder.ToTable("DisbursementTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.NameFr)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasMany(x => x.Disbursements)
            .WithOne(x => x.DisbursementType)
            .HasForeignKey(x => x.DisbursementTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("a1f5c8e2-3d4b-4f6a-9c2e-1b2c3d4e5f60"),
                Code = "A1",
                Name = "Direct payment",
                NameFr = "Paiement direct",
                Description = "Description for Type 1 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("b2f6d9f3-4e5c-5g7b-0d3f-2c3d4e5f6g70"),
                Code = "A1",
                Name = "Replenishment of a Special Account",
                NameFr = "Réapprovisionnement d'un compte spécial",
                Description = "Description for Type 2 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("c3g7e0g4-5f6d-6h8c-1e4g-3d4e5f6g7h80"),
                Code = "A1",
                Name = "Justification only",
                NameFr = "Justification",
                Description = "Description for Type 3 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("d4h8f1h5-6g7e-7i9d-2f5h-4e5f6g7h8i90"),
                Code = "A2",
                Name = "Declare Expenditures",
                NameFr = "Déclarer les dépenses",
                Description = "Description for Type 4 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("e5i9g2i6-7h8f-8j0e-3g6i-5f6g7h8i9j00"),
                Code = "A3",
                Name = "Estimate Budgeted Activities",
                NameFr = "Estimation des activités budgétisées",
                Description = "Description for Type 5 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("f6j0h3j7-8i9g-9k1f-4h7j-6g7h8i9j0k10"),
                Code = "B1",
                Name = "Reimbursement of a guarantee",
                NameFr = "Remboursement d'une garantie",
                Description = "Description for Type 6 Disbursement"
            }
        );
    }
}
