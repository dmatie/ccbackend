using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public sealed class DisbursementPermissionConfiguration : IEntityTypeConfiguration<DisbursementPermissionEntity>
{
    public void Configure(EntityTypeBuilder<DisbursementPermissionEntity> builder)
    {
        builder.ToTable("DisbursementPermissions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.BusinessProfileId)
            .IsRequired();

        builder.Property(e => e.FunctionId)
            .IsRequired();

        builder.Property(e => e.CanConsult)
            .IsRequired();

        builder.Property(e => e.CanSubmit)
            .IsRequired();

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.UpdatedDate);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(255);

        builder.HasOne(e => e.BusinessProfile)
            .WithMany()
            .HasForeignKey(e => e.BusinessProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Function)
            .WithMany()
            .HasForeignKey(e => e.FunctionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.BusinessProfileId, e.FunctionId })
            .IsUnique()
            .HasDatabaseName("IX_DisbursementPermissions_BusinessProfile_Function");
    }
}
