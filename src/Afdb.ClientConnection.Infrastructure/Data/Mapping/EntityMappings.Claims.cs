using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using System.Linq;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal partial class EntityMappings
{
    public static ClaimEntity MapClaimToEntity(Claim source)
    {
        var entity = new ClaimEntity
        {
            Id = source.Id,
            ClaimTypeId = source.ClaimTypeId,
            CountryId = source.CountryId,
            UserId = source.UserId,
            ClosedAt = source.ClosedAt,
            Status = source.Status,
            Comment = source.Comment,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy,
            UpdatedAt = source.UpdatedAt,
            UpdatedBy = source.UpdatedBy
        };

        if (source.Processes.Count > 0)
        {
            entity.Processes = [];
            foreach (var item in source.Processes)
            {
                entity.Processes.Add(new()
                {
                    Id = item.Id,
                    ClaimId = item.ClaimId,
                    Comment = item.Comment,
                    UserId = item.UserId,
                    Status = item.Status,
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

    public static void UpdateClaimEntityFromDomain(ClaimEntity entity, Claim source)
    {
        entity.Status = source.Status;
        entity.ClosedAt = source.ClosedAt;
        entity.CreatedAt = source.CreatedAt;
        entity.CreatedBy = source.CreatedBy;
        entity.UpdatedAt = source.UpdatedAt;
        entity.UpdatedBy = source.UpdatedBy;

        if (source.Processes.Count > 0)
        {
            var existingIds = entity.Processes.Select(p => p.Id).ToHashSet();

            var itemsToAdd = source.Processes.Where(item => !existingIds.Contains(item.Id)).ToList();

            foreach (var item in itemsToAdd)
            {
                entity.Processes.Add(MapClaimProcessToEntity(item));
            }
        }

        entity.DomainEvents = source.DomainEvents.ToList();
    }

    public static ClaimProcessEntity MapClaimProcessToEntity(ClaimProcess source)
    {
        var entity = new ClaimProcessEntity
        {
            ClaimId = source.ClaimId,
            UserId = source.UserId,
            Status = source.Status,
            Comment = source.Comment,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy,
            UpdatedAt = source.UpdatedAt,
            UpdatedBy = source.UpdatedBy
        };

        return entity;
    }
}
