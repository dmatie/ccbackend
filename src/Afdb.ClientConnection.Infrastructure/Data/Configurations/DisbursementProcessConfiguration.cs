using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementProcessConfiguration : IEntityTypeConfiguration<DisbursementProcessEntity>
{
    public void Configure(EntityTypeBuilder<DisbursementProcessEntity> builder)
    {
        builder.ToTable("DisbursementProcesses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(2000);

        builder.Property(x => x.DocumentUrl)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Disbursement)
            .WithMany(x => x.Processes)
            .HasForeignKey(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
