using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class AccessRequestDocumentConfiguration : IEntityTypeConfiguration<AccessRequestDocumentEntity>
{
    public void Configure(EntityTypeBuilder<AccessRequestDocumentEntity> builder)
    {
        builder.ToTable("AccessRequestDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DocumentUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.AccessRequest)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.AccessRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
