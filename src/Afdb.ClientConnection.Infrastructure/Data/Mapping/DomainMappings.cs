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
                Id = entity.Id,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                FunctionId = entity.FunctionEntityId,
                CountryId = entity.CountryEntityId,
                BusinessProfileId = entity.BusinessProfileEntityId,
                FinancingTypeId = entity.FinancingTypeEntityId,
                Status = entity.Status,
                ProcessedById = entity.ProcessedById,
                ProcessedDate = entity.ProcessedDate,
                ProcessingComments = entity.ProcessingComments,
                RequestedDate= entity.RequestedDate,
                ProcessedBy = entity.ProcessedBy != null ? MapUser(entity.ProcessedBy) : null,
                Function = entity.Function != null ? MapFunction(entity.Function) : null,
                Country = entity.Country != null ? MapCountry(entity.Country) : null,
                BusinessProfile = entity.BusinessProfile != null ? MapBusinessProfile(entity.BusinessProfile) : null,
                FinancingType = entity.FinancingType != null ? MapFinancingType(entity.FinancingType) : null,
                Projects = entity.Projects?.Select(MapProject).ToList() ?? new List<AccessRequestProject>(),
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedBy = entity.UpdatedBy,
                UpdatedAt = entity.UpdatedAt,
            }
        );

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

    public static DisbursementPermission? ToDomain(this DisbursementPermissionEntity? entity)
    {
        if (entity == null) return null;

        return DisbursementPermission.Create(
            entity.BusinessProfileId,
            entity.FunctionId,
            entity.CanConsult,
            entity.CanSubmit
        );
    }
}
