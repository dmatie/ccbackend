using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementA1Configuration : IEntityTypeConfiguration<DisbursementA1Entity>
{
    public void Configure(EntityTypeBuilder<DisbursementA1Entity> builder)
    {
        builder.ToTable("DisbursementA1");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PaymentPurpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BeneficiaryName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BeneficiaryBankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryBankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BeneficiaryAccountNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.BeneficiarySwiftCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.IntermediaryBankName)
            .HasMaxLength(200);

        builder.Property(x => x.IntermediaryBankSwiftCode)
            .HasMaxLength(50);

        builder.Property(x => x.SpecialInstructions)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA1)
            .HasForeignKey<DisbursementA1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
