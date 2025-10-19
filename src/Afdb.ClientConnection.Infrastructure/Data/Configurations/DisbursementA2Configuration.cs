using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementA2Configuration : IEntityTypeConfiguration<DisbursementA2Entity>
{
    public void Configure(EntityTypeBuilder<DisbursementA2Entity> builder)
    {
        builder.ToTable("DisbursementA2");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReimbursementPurpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ClaimantName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ClaimantAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ClaimantBankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ClaimantBankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ClaimantAccountNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ClaimantSwiftCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.ExpenseDate)
            .IsRequired();

        builder.Property(x => x.ExpenseDescription)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.SupportingDocuments)
            .HasMaxLength(1000);

        builder.Property(x => x.SpecialInstructions)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA2)
            .HasForeignKey<DisbursementA2Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
