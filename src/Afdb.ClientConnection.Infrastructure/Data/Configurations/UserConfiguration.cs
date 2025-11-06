using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.HasIndex(e => e.EntraIdObjectId)
            .IsUnique()
            .HasFilter("[EntraIdObjectId] IS NOT NULL");

        builder.Property(e => e.Role)
            .HasConversion<string>();

        builder.HasMany(e => e.CountryAdmins)
            .WithOne(ca => ca.User)
            .HasForeignKey(ca => ca.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}