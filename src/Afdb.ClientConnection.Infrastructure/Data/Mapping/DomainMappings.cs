using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static AccessRequest MapToDomain(AccessRequestEntity entity)
    {
        if (entity == null) return null;

        var accessRequest = new AccessRequest(
            new AccessRequestLoadParam
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                FunctionId = entity.FunctionEntityId,
                CountryId = entity.CountryEntityId,
                BusinessProfileId = entity.BusinessProfileEntityId,
                FinancingTypeId = entity.FinancingTypeEntityId,
                Function = entity.Function != null ? MapFunction(entity.Function) : null,
                Country = entity.Country != null ? MapCountry(entity.Country) : null,
                BusinessProfile = entity.BusinessProfile != null ? MapBusinessProfile(entity.BusinessProfile) : null,
                FinancingType = entity.FinancingType != null ? MapFinancingType(entity.FinancingType) : null,
                Projects = entity.Projects?.Select(MapProject).ToList() ?? new List<AccessRequestProject>(),
                CreatedBy = entity.CreatedBy
            }
        );

        // Mapper les propriétés supplémentaires
        if (entity.ProcessedBy != null)
            accessRequest.GetType().GetProperty("ProcessedBy")?.SetValue(accessRequest, MapUser(entity.ProcessedBy));

        // Mapper les propriétés simples
        accessRequest.GetType().GetProperty("Status")?.SetValue(accessRequest, entity.Status);
        accessRequest.GetType().GetProperty("RequestedDate")?.SetValue(accessRequest, entity.RequestedDate);
        accessRequest.GetType().GetProperty("ProcessedDate")?.SetValue(accessRequest, entity.ProcessedDate);
        accessRequest.GetType().GetProperty("ProcessedById")?.SetValue(accessRequest, entity.ProcessedById);
        accessRequest.GetType().GetProperty("ProcessingComments")?.SetValue(accessRequest, entity.ProcessingComments);
        accessRequest.GetType().GetProperty("EntraIdObjectId")?.SetValue(accessRequest, entity.EntraIdObjectId);

        typeof(AccessRequest).GetProperty("CreatedAt")!.SetValue(accessRequest, entity.CreatedAt);
        typeof(AccessRequest).GetProperty("CreatedBy")!.SetValue(accessRequest, entity.CreatedBy);
        typeof(AccessRequest).GetProperty("UpdatedAt")!.SetValue(accessRequest, entity.UpdatedAt);
        typeof(AccessRequest).GetProperty("UpdatedBy")!.SetValue(accessRequest, entity.UpdatedBy);
        typeof(AccessRequest).GetProperty("Id")!.SetValue(accessRequest, entity.Id);

        return accessRequest;
    }

    // Exemple de mapping pour les entités liées
    public static User MapUser(UserEntity entity)
    {
        return new User(email: entity.Email, firstName: entity.FirstName, lastName: entity.LastName,
            role: entity.Role, entraIdObjectId: entity.EntraIdObjectId ?? string.Empty,
            createdBy: entity.CreatedBy, organizationName: entity.OrganizationName);
    }

    public static Function MapFunction(FunctionEntity entity)
    {
        return new Function(entity.Id, entity.Name, entity.Code, entity.Description, entity.CreatedBy);
    }

    public static Country MapCountry(CountryEntity entity)
    {
        return new Country(entity.Id, entity.Name, entity.NameFr, entity.Code, entity.CreatedBy);
    }

    public static BusinessProfile MapBusinessProfile(BusinessProfileEntity entity)
    {
        return new BusinessProfile(entity.Id, entity.Name, entity.Description, entity.CreatedBy);
    }

    public static FinancingType MapFinancingType(FinancingTypeEntity entity)
    {
        return new FinancingType(entity.Id, entity.Name, entity.Code, entity.Description, entity.CreatedBy);
    }

    public static AccessRequestProject MapProject(AccessRequestProjectEntity entity)
    {
        // Mapper les propriétés nécessaires
        return new AccessRequestProject(entity.AccessRequestId,entity.SapCode);
    }
}
