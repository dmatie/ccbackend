using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementConfiguration : IEntityTypeConfiguration<DisbursementEntity>
{
    public void Configure(EntityTypeBuilder<DisbursementEntity> builder)
    {
        builder.ToTable("Disbursements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.RequestNumber)
            .IsUnique();

        builder.Property(x => x.SapCodeProject)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LoanGrantNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.ProcessedByDate);

        builder.Property(x => x.DocumentsUrl)
            .HasMaxLength(1000);

        builder.HasOne(x => x.DisbursementType)
            .WithMany()
            .HasForeignKey(x => x.DisbursementTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ProcessedByUser)
            .WithMany()
            .HasForeignKey(x => x.ProcessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Processes)
            .WithOne(x => x.Disbursement)
            .HasForeignKey(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Documents)
            .WithOne(x => x.Disbursement)
            .HasForeignKey(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DisbursementA1)
            .WithOne(x => x.Disbursement)
            .HasForeignKey<DisbursementA1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DisbursementA2)
            .WithOne(x => x.Disbursement)
            .HasForeignKey<DisbursementA2Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DisbursementA3)
            .WithOne(x => x.Disbursement)
            .HasForeignKey<DisbursementA3Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DisbursementB1)
            .WithOne(x => x.Disbursement)
            .HasForeignKey<DisbursementB1Entity>(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
