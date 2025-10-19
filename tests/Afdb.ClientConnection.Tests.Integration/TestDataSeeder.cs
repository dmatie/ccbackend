using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Afdb.ClientConnection.Tests.Integration;

public static class TestDataSeeder
{
    public static async Task<(FunctionEntity? Function, CountryEntity?
        Country, BusinessProfileEntity? Profile, FinancingTypeEntity? financingType)>
       SeedReferenceDataAsync(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClientConnectionDbContext>();

        // Seed Function
        if (!await db.Functions.AnyAsync())
        {
            db.Functions.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = "ADB Desk Office",
                Description = "African Development Bank Desk Office",
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            });
        }

        // Seed Country
        if (!await db.Countries.AnyAsync())
        {
            db.Countries.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = "Algeria",
                NameFr = "Algérie",
                Code = "DZA",
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            });
        }

        // Seed BusinessProfile
        if (!await db.BusinessProfiles.AnyAsync())
        {
            db.BusinessProfiles.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = "Executing Agency",
                Description = "Executing Agency",
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            });
        }

        // Seed BusinessProfile
        if (!await db.FinancingTypes.AnyAsync())
        {
            db.FinancingTypes.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = "Private",
                Description = "Executing Agency",
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            });
        }


        await db.SaveChangesAsync();

        // Vérifie s’il existe déjà pour éviter les doublons entre tests
        var function = await db.Functions.FirstOrDefaultAsync() ?? null;

        var country = await db.Countries.FirstOrDefaultAsync() ?? null;

        var profile = await db.BusinessProfiles.FirstOrDefaultAsync() ?? null;
        
        var financingType = await db.FinancingTypes.FirstOrDefaultAsync() ?? null;

        await db.SaveChangesAsync();
        return (function, country, profile, financingType);
    }


    public static async Task<string> GetOtpAsync(IServiceScopeFactory scopeFactory, string email)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClientConnectionDbContext>();

        return await db.OtpCodes
            .Where(o => o.Email == email && o.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => o.Code)
            .FirstOrDefaultAsync() ?? string.Empty;
    }

    public static async Task CreateUser(
       IServiceScopeFactory scopeFactory, string email, string firstName, string lastName)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClientConnectionDbContext>();
        if (!await db.Users.AnyAsync(u => u.Email == email))
        {
            db.Users.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                EntraIdObjectId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid().ToString(),
                Role = Domain.Enums.UserRole.ExternalUser,
                OrganizationName = "Existing Org",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = Guid.NewGuid().ToString(),
            });
            await db.SaveChangesAsync();
        }
    }
}