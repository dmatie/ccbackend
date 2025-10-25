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

        builder.Property(x => x.BeneficiaryBpNumber)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryContactPerson)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BeneficiaryCountryId);

        builder.Property(x => x.BeneficiaryEmail)
            .IsRequired()
            .HasMaxLength(200);


        builder.Property(x => x.CorrespondentBankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CorrespondentBankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.CorrespondentBankCountryId);

        builder.Property(x => x.CorrespondantAccountNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CorrespondentBankSwiftCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");


        builder.Property(x => x.SignatoryName)
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryContactPerson)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.SignatoryCountryId);

        builder.Property(x => x.SignatoryEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryPhone)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA1)
            .HasForeignKey<DisbursementA1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.BeneficiaryCountry)
            .WithMany()
            .HasForeignKey(x => x.BeneficiaryCountryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CorrespondentBankCountry)
            .WithMany()
            .HasForeignKey(x => x.CorrespondentBankCountryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SignatoryCountry)
            .WithMany()
            .HasForeignKey(x => x.SignatoryCountryId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
