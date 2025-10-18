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

        builder.Property(x => x.BusinessPartnerNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.BeneficiaryName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryContactPerson)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BeneficiaryCity)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BankAccountNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.BankSwiftCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.SignatoryName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryContactPerson)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SignatoryPhone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.SignatoryAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.SignatoryCity)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA1)
            .HasForeignKey<DisbursementA1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.BeneficiaryCountry)
            .WithMany()
            .HasForeignKey(x => x.BeneficiaryCountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.BankCountry)
            .WithMany()
            .HasForeignKey(x => x.BankCountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SignatoryCountry)
            .WithMany()
            .HasForeignKey(x => x.SignatoryCountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
