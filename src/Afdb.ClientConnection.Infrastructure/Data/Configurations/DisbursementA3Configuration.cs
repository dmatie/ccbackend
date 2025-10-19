using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementA3Configuration : IEntityTypeConfiguration<DisbursementA3Entity>
{
    public void Configure(EntityTypeBuilder<DisbursementA3Entity> builder)
    {
        builder.ToTable("DisbursementA3");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AdvancePurpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.RecipientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.RecipientAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.RecipientBankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.RecipientBankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.RecipientAccountNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.RecipientSwiftCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(x => x.ExpectedUsageDate)
            .IsRequired();

        builder.Property(x => x.JustificationForAdvance)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.RepaymentTerms)
            .HasMaxLength(1000);

        builder.Property(x => x.SpecialInstructions)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA3)
            .HasForeignKey<DisbursementA3Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
