using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static Country MapCountry(CountryEntity entity)
    {
        Country country = new(new CountryLoadParam
        {
            Id = entity.Id,
            Name = entity.Name,
            NameFr = entity.NameFr,
            Code = entity.Code,
            IsActive = entity.IsActive,
            CountryAdmins = entity.CountryAdmins?.Select(MapCountryAdminToDomain).ToList() ?? [],
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
        });

        return country;
    }

    public static CountryAdmin MapCountryAdminToDomain(CountryAdminEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var destination = new CountryAdmin(new CountryAdminLoadParam
        {
            Id = entity.Id,
            CountryId = entity.CountryId,
            UserId = entity.UserId,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
        });

        return destination;
    }
}
