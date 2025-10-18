using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class CountryAdminConfiguration : IEntityTypeConfiguration<CountryAdminEntity>
{
    public void Configure(EntityTypeBuilder<CountryAdminEntity> builder)
    {
        builder.ToTable("CountryAdmins");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsActive);

        builder.HasOne(x => x.User)
            .WithMany(x => x.CountryAdmins)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Country)
            .WithMany(c=> c.CountryAdmins)
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);     
    }
}