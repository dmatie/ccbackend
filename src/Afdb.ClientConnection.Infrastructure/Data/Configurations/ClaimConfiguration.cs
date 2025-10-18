using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class ClaimConfiguration : IEntityTypeConfiguration<ClaimEntity>
{
    public void Configure(EntityTypeBuilder<ClaimEntity> builder)
    {
        builder.ToTable("Claims");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.ClosedAt);

        builder.Property(e => e.Status);

        builder.HasOne(x => x.ClaimType)
            .WithMany(x => x.Claims)
            .HasForeignKey(x => x.ClaimTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Country)
            .WithMany(c=> c.Claims)
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(u=>u.Claims)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Processes)
            .WithOne(x => x.Claim)
            .HasForeignKey(x => x.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}