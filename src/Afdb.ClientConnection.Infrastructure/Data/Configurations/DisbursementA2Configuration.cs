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

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Contractor)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.GoodDescription)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.ContractBorrowerReference)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ContractAfDBReference)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ContractValue)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ContractBankShare)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ContractAmountPreviouslyPaid)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.InvoiceRef)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.InvoiceDate)
            .IsRequired();

        builder.Property(x => x.InvoiceAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA2)
            .HasForeignKey<DisbursementA2Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.GoodOriginCountry)
            .WithMany()
            .HasForeignKey(x => x.GoodOriginCountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
