using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class ClaimProcessConfiguration : IEntityTypeConfiguration<ClaimProcessEntity>
{
    public void Configure(EntityTypeBuilder<ClaimProcessEntity> builder)
    {
        builder.ToTable("ClaimProcesses");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Status)
            .HasConversion<string>();


        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasOne(x => x.Claim)
            .WithMany(x => x.Processes)
            .HasForeignKey(x => x.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u=>u.ClaimProcesses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}