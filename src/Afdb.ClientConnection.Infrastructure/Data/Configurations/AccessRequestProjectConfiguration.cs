using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class AccessRequestProjectConfiguration : IEntityTypeConfiguration<AccessRequestProjectEntity>
{
    public void Configure(EntityTypeBuilder<AccessRequestProjectEntity> builder)
    {
        builder.ToTable("AccessRequestsProjects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SapCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt);

        builder.HasOne(x => x.AccessRequest)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.AccessRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index pour optimiser les recherches par AccessRequestId
        builder.HasIndex(x => new { x.AccessRequestId, x.SapCode }).IsUnique();
    }
}
