using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static CountryAdmin MapCountryAdminToDomain(CountryAdminEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var destination = new CountryAdmin(new CountryAdminLoadParam
        {
            Id = entity.Id,
            CountryId = entity.CountryId,
            UserId = entity.UserId,
            Country = MapCountry(entity.Country),
            User = MapUser(entity.User),
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
        });

        return destination;
    }
}
