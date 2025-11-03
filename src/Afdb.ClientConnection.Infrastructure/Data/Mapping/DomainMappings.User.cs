using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static User MapUserToDomain(UserEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var destination = new User(new UserLoadParam
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Role = entity.Role,
            IsActive = entity.IsActive,
            EntraIdObjectId = entity.EntraIdObjectId,
            OrganizationName = entity.OrganizationName,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
            Countries= entity.CountryAdmins?.Select(MapCountryAdminToDomain).ToList() ?? []
        });

        return destination;
    }
}
