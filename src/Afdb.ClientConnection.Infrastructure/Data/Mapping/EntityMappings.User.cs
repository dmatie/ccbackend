using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using System.Linq;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal partial class EntityMappings
{
    public static UserEntity MapUserToEntity(User source)
    {
        var entity = new UserEntity
        {
            Id = source.Id,
            Email = source.Email,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Role = source.Role,
            IsActive = source.IsActive,
            EntraIdObjectId = source.EntraIdObjectId,
            OrganizationName = source.OrganizationName,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy,
            UpdatedAt = source.UpdatedAt,
            UpdatedBy = source.UpdatedBy
        };

        if (source.Countries.Count > 0)
        {
            entity.CountryAdmins = [];
            foreach (var item in source.Countries)
            {
                entity.CountryAdmins.Add(new()
                {
                    CountryId = item.CountryId,
                    UserId = source.Id,
                    IsActive = item.IsActive,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    UpdatedAt = item.UpdatedAt,
                    UpdatedBy = item.UpdatedBy
                });
            }
        }

        entity.DomainEvents = source.DomainEvents.ToList();

        return entity;
    }

    public static void UpdateUserEntityFromDomain(UserEntity entity, User source)
    {
        entity.FirstName = source.FirstName;
        entity.LastName = source.LastName;
        entity.Role = source.Role;
        entity.IsActive = source.IsActive;
        entity.CreatedAt = source.CreatedAt;
        entity.CreatedBy = source.CreatedBy;
        entity.UpdatedAt = source.UpdatedAt;
        entity.UpdatedBy = source.UpdatedBy;

        // Synchronisation des pays (CountryAdmins)
        var sourceIds = source.Countries.Select(ca => ca.Id).ToHashSet();
        var entityIds = entity.CountryAdmins.Select(ca => ca.Id).ToHashSet();

        // Suppression des pays retirés
        var toRemove = entity.CountryAdmins.Where(ca => !sourceIds.Contains(ca.Id)).ToList();
        foreach (var item in toRemove)
        {
            entity.CountryAdmins.Remove(item);
        }

        // Ajout des nouveaux pays
        var toAdd = source.Countries.Where(ca => !entityIds.Contains(ca.Id)).ToList();
        foreach (var item in toAdd)
        {
            entity.CountryAdmins.Add(new()
            {
                CountryId = item.CountryId,
                UserId = source.Id,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                UpdatedAt = item.UpdatedAt,
                UpdatedBy = item.UpdatedBy
            });
        }

        entity.DomainEvents = source.DomainEvents.ToList();
    }
}
