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

        builder.Property(x => x.GuaranteeDetails)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ConfirmingBank)
            .IsRequired()
            .HasMaxLength(500);


        builder.Property(x => x.IssuingBankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IssuingBankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ExpiryDate)
            .IsRequired();

        builder.Property(x => x.BeneficiaryName)
            .IsRequired()
            .HasMaxLength(200);


        builder.Property(x => x.BeneficiaryBPNumber)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryAFDBContract)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryBankAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.BeneficiaryCity)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BeneficiaryCountryId);

        builder.Property(x => x.BeneficiaryLcContractRef)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExecutingAgencyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExecutingAgencyContactPerson)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExecutingAgencyAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ExecutingAgencyCity)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExecutingAgencyCountryId)
            .IsRequired();

        builder.Property(x => x.ExecutingAgencyEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExecutingAgencyPhone)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementB1)
            .HasForeignKey<DisbursementB1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.BeneficiaryCountry)
            .WithMany()
            .HasForeignKey(x => x.BeneficiaryCountryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ExecutingAgencyCountry)
            .WithMany()
            .HasForeignKey(x => x.ExecutingAgencyCountryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
