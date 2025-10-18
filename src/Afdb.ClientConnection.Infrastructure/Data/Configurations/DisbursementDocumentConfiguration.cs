using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementDocumentConfiguration : IEntityTypeConfiguration<DisbursementDocumentEntity>
{
    public void Configure(EntityTypeBuilder<DisbursementDocumentEntity> builder)
    {
        builder.ToTable("DisbursementDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DocumentUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(x => x.Disbursement)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.DisbursementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
