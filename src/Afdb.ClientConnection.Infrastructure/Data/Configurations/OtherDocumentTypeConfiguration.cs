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
    }
}
