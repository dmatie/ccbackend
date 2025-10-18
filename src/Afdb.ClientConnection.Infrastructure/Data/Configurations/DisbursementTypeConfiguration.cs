using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class DisbursementTypeConfiguration : IEntityTypeConfiguration<DisbursementTypeEntity>
{
    public void Configure(EntityTypeBuilder<DisbursementTypeEntity> builder)
    {
        builder.ToTable("DisbursementTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.NameFr)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.FormCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.FormCode)
            .IsUnique();
    }
}
