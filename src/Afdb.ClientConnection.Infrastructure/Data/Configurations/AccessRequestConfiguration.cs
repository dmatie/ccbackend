using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class AccessRequestConfiguration : IEntityTypeConfiguration<AccessRequestEntity>
{
    public void Configure(EntityTypeBuilder<AccessRequestEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Email);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasDefaultValue("123456")
            .HasMaxLength(10);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Status);

        builder.Property(e => e.FunctionEntityId);
        builder.Property(e => e.CountryEntityId);
        builder.Property(e => e.BusinessProfileEntityId);

        // Navigation properties
        builder.HasOne(e => e.ProcessedBy)
            .WithMany(u => u.ProcessedAccessRequests)
            .HasForeignKey(e => e.ProcessedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Function)
            .WithMany()
            .HasForeignKey(e => e.FunctionEntityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryEntityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.BusinessProfile)
            .WithMany()
            .HasForeignKey(e => e.BusinessProfileEntityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.FinancingType)
           .WithMany()
           .HasForeignKey(e => e.FinancingTypeEntityId)
           .OnDelete(DeleteBehavior.SetNull);
    }
}
