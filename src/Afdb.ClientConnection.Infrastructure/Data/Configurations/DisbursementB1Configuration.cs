using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementB1Configuration : IEntityTypeConfiguration<DisbursementB1Entity>
{
    public void Configure(EntityTypeBuilder<DisbursementB1Entity> builder)
    {
        builder.ToTable("DisbursementB1");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.GuaranteePurpose)
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

        builder.Property(x => x.GuaranteeAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.ValidityStartDate)
            .IsRequired();

        builder.Property(x => x.ValidityEndDate)
            .IsRequired();

        builder.Property(x => x.GuaranteeTermsAndConditions)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.SpecialInstructions)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementB1)
            .HasForeignKey<DisbursementB1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
