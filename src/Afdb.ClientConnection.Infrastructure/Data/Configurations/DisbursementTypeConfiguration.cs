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

        builder.HasIndex(x => x.Code);

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
                Id = Guid.Parse("65616f7b-b385-45f7-a5fc-87dbf5a68d3b"),
                Code = "A1",
                Name = "Direct payment",
                NameFr = "Paiement direct",
                Description = "Description for Type 1 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("691ca504-2252-4039-a4cc-86228432ccea"),
                Code = "A1",
                Name = "Replenishment of a Special Account",
                NameFr = "Réapprovisionnement d'un compte spécial",
                Description = "Description for Type 2 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("84e690ed-c29c-4b34-b113-34a5c5d61b6f"),
                Code = "A1",
                Name = "Justification only",
                NameFr = "Justification",
                Description = "Description for Type 3 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("9509bfe3-5ba7-4302-b7fb-11ca7007fa39"),
                Code = "A2",
                Name = "Declare Expenditures",
                NameFr = "Déclarer les dépenses",
                Description = "Description for Type 4 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("76f95c38-b800-4f4c-a0c3-cada31703bd5"),
                Code = "A3",
                Name = "Estimate Budgeted Activities",
                NameFr = "Estimation des activités budgétisées",
                Description = "Description for Type 5 Disbursement"
            },
            new DisbursementTypeEntity
            {
                Id = Guid.Parse("3fcde576-d6e1-4145-a52e-0da14f8e08c1"),
                Code = "B1",
                Name = "Reimbursement of a guarantee",
                NameFr = "Remboursement d'une garantie",
                Description = "Description for Type 6 Disbursement"
            }
        );
    }
}
