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

        builder.Property(x => x.Contractor)
            .IsRequired()
            .HasMaxLength(200);


        builder.Property(x => x.GoodDescription)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.GoodOrginCountryId);

        builder.Property(x => x.ContractBorrowerReference)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContractAfDBReference)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContractValue)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContractBankShare)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContractAmountPreviouslyPaid)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.InvoiceRef)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.InvoiceDate)
            .IsRequired();

        builder.Property(x => x.InvoiceAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PaymentDateOfPayment)
            .IsRequired();

        builder.Property(x => x.PaymentAmountWithdrawn)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA2)
            .HasForeignKey<DisbursementA2Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GoodOrginCountry)
            .WithMany()
            .HasForeignKey(x => x.GoodOrginCountryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
