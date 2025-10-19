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

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.NameFr)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasMany(x => x.Disbursements)
            .WithOne(x => x.DisbursementType)
            .HasForeignKey(x => x.DisbursementTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
