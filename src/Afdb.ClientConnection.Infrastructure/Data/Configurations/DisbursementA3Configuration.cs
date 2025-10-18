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

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PeriodForUtilization)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.GoodDescription)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.GoodQuantity)
            .IsRequired();

        builder.Property(x => x.AnnualBudget)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.BankShare)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.AdvanceRequested)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Disbursement)
            .WithOne(x => x.DisbursementA3)
            .HasForeignKey<DisbursementA3Entity>(x => x.DisbursementId)
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
