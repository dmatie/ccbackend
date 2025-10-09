using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class FunctionConfiguration : IEntityTypeConfiguration<FunctionEntity>
{
    public void Configure(EntityTypeBuilder<FunctionEntity> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Code).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Description).HasMaxLength(500);
        builder.Property(f => f.IsActive).HasDefaultValue(true);
        builder.HasIndex(f => f.Name).IsUnique();
        builder.HasIndex(f => f.Code).IsUnique();

        builder.HasData(
            new FunctionEntity
            {
                Id = Guid.Parse("1a2b3c4d-0001-4e5f-9012-abcdef123456"),
                Name = "ADB Desk Office",
                Code = "ADB_DESK_OFFICE",
                Description = "African Development Bank Desk Office",
                IsActive = true
            },
            new FunctionEntity
            {
                Id = Guid.Parse("1a2b3c4d-0002-4e5f-9012-abcdef123456"),
                Name = "Director/Finance and Administrative Manager",
                Code = "DIRECTOR_FIN_ADMIN",
                Description = "Director responsible for finance and administration",
                IsActive = true
            },
            new FunctionEntity
            {
                Id = Guid.Parse("1a2b3c4d-0003-4e5f-9012-abcdef123456"),
                Name = "Ministry Staff",
                Code = "MINISTRY_STAFF",
                Description = "Staff from the Ministry",
                IsActive = true
            },
            new FunctionEntity
            {
                Id = Guid.Parse("1a2b3c4d-0004-4e5f-9012-abcdef123456"),
                Name = "Project Accountant",
                Code = "PROJECT_ACCOUNTANT",
                Description = "Accountant working on projects",
                IsActive = true
            },
            new FunctionEntity
            {
                Id = Guid.Parse("1a2b3c4d-0005-4e5f-9012-abcdef123456"),
                Name = "Project Coordinator",
                Code = "PROJECT_COORDINATOR",
                Description = "Coordinator for project activities",
                IsActive = true
            },
            new FunctionEntity
            {
                Id = Guid.Parse("1a2b3c4d-0006-4e5f-9012-abcdef123456"),
                Name = "Other",
                Code = "OTHER",
                Description = "Other functions",
                IsActive = true
            }
);


    }
}
