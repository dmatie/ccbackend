using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class OtherDocumentConfiguration : IEntityTypeConfiguration<OtherDocumentEntity>
{
    public void Configure(EntityTypeBuilder<OtherDocumentEntity> builder)
    {
        builder.ToTable("OtherDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Year)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.SAPCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LoanNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.OtherDocumentType)
            .WithMany(x => x.OtherDocuments)
            .HasForeignKey(x => x.OtherDocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Files)
            .WithOne(x => x.OtherDocument)
            .HasForeignKey(x => x.OtherDocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
