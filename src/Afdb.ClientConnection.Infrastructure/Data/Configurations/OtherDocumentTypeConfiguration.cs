using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class OtherDocumentTypeConfiguration : IEntityTypeConfiguration<OtherDocumentTypeEntity>
{
    public void Configure(EntityTypeBuilder<OtherDocumentTypeEntity> builder)
    {
        builder.ToTable("OtherDocumentTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.NameFr)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasMany(x => x.OtherDocuments)
            .WithOne(x => x.OtherDocumentType)
            .HasForeignKey(x => x.OtherDocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new OtherDocumentTypeEntity
            {
                Id = Guid.Parse("eae933da-22e9-4735-96c4-4483157bb27f"),
                Name = "specimen signature",
                NameFr = "Spécimen de signature",
                IsActive = true,
                CreatedBy = "System",
                CreatedAt = new DateTime(2025,12,14,0,0,0,DateTimeKind.Utc),
            },
            new OtherDocumentTypeEntity
            {
                Id = Guid.Parse("25364bf0-87ae-4a5e-9897-c1eb5351ad5f"),
                Name = "Other",
                NameFr = "Autre",
                IsActive = true,
                CreatedBy = "System",
                CreatedAt = new DateTime(2025, 12, 14, 0, 0, 0, DateTimeKind.Utc),
            }
         );
    }
}
