using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static Claim MapClaimToDomain(ClaimEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var destination = new Claim(new ClaimLoadParam
        {
            Id = entity.Id,
            ClaimTypeId = entity.ClaimTypeId,
            CountryId = entity.CountryId,
            UserId = entity.UserId,
            ClaimType = entity.ClaimType != null ? MapClaimTypeToDomain(entity.ClaimType) : null,
            Country = entity.Country != null ? MapCountry(entity.Country) : null,
            User = entity.User != null ? MapUser(entity.User) : null,
            Processes = entity.Processes?.Select(MapClaimProcessToDomain).ToList() ?? [],
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
        });

        return destination;
    }

    public static ClaimType MapClaimTypeToDomain(ClaimTypeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        ClaimType claimType = new ClaimType(new ClaimTypeLoadParam
        {
            Id = entity.Id,
            Name = entity.Name,
            NameFr = entity.NameFr,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });

        return claimType;
    }

    public static ClaimProcess? MapClaimProcessToDomain(ClaimProcessEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        ClaimProcess destination = new ClaimProcess(new ClaimProcessLoadParam
        {
            Id = entity.Id,
            ClaimId = entity.ClaimId,
            UserId = entity.UserId,
            User = entity.User != null ? MapUser(entity.User) : null,
            Comment = entity.Comment,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });

        return destination;
    }
}
