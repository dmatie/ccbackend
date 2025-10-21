using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class ClaimTypeConfiguration : IEntityTypeConfiguration<ClaimTypeEntity>
{
    public void Configure(EntityTypeBuilder<ClaimTypeEntity> builder)
    {
        builder.ToTable("ClaimTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.NameFr)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.HasMany(x => x.Claims)
            .WithOne(x => x.ClaimType)
            .HasForeignKey(x => x.ClaimTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new ClaimTypeEntity
            {
                Id = Guid.Parse("fd97a4df-40ef-4581-b5bf-983d56e70b3d"),
                Name = "Payment not received",
                NameFr="Paiement non reçu",
                Description="",
            },
            new ClaimTypeEntity
            {
                Id = Guid.Parse("57cf2666-198c-41a9-b828-8f0651da76ed"),
                Name = "Disbursment",
                NameFr = "Décaissement",
                Description = "",
            },
            new ClaimTypeEntity
            {
                Id = Guid.Parse("f671b0d7-ee85-48d8-a58b-0029660b2cad"),
                Name = "Other",
                NameFr = "Autre",
                Description = "",
            }
            );

    }
}