using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class OtherDocumentFileConfiguration : IEntityTypeConfiguration<OtherDocumentFileEntity>
{
    public void Configure(EntityTypeBuilder<OtherDocumentFileEntity> builder)
    {
        builder.ToTable("OtherDocumentFiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.FileSize)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.UploadedAt)
            .IsRequired();

        builder.Property(x => x.UploadedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.OtherDocument)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.OtherDocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
