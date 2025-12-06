using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial  class EntityMappings
{
    public static AccessRequestEntity MapToEntity(AccessRequest accessRequest)
    {
        var entity = new AccessRequestEntity
        {
            Id = accessRequest.Id,
            Email = accessRequest.Email,
            FirstName = accessRequest.FirstName,
            LastName = accessRequest.LastName,
            Code = accessRequest.Code,
            Status = accessRequest.Status,
            RequestedDate = accessRequest.RequestedDate,
            ProcessedDate = accessRequest.ProcessedDate,
            ProcessedById = accessRequest.ProcessedById,
            ProcessingComments = accessRequest.ProcessingComments,
            EntraIdObjectId = accessRequest.EntraIdObjectId,
            FunctionEntityId = accessRequest.FunctionId,
            CountryEntityId = accessRequest.CountryId,
            BusinessProfileEntityId = accessRequest.BusinessProfileId,
            FinancingTypeEntityId = accessRequest.FinancingTypeId,
            CreatedAt = accessRequest.CreatedAt,
            CreatedBy = accessRequest.CreatedBy,
            UpdatedAt = accessRequest.UpdatedAt,
            UpdatedBy = accessRequest.UpdatedBy
        };

        if (accessRequest.Projects.Count > 0)
        {
            entity.Projects = [];
            foreach (var item in accessRequest.Projects)
            {
                entity.Projects.Add(new()
                {
                    SapCode = item.SapCode,
                    ProjectTitle = item.ProjectTitle,
                });
            }
        }

        entity.DomainEvents = accessRequest.DomainEvents.ToList();

        return entity;
    }

    public static void UpdateEntityFromDomain(AccessRequestEntity entity, AccessRequest accessRequest)
    {
        entity.Status = accessRequest.Status;
        entity.ProcessedDate = accessRequest.ProcessedDate;
        entity.ProcessedById = accessRequest.ProcessedById;
        entity.ProcessingComments = accessRequest.ProcessingComments;
        entity.EntraIdObjectId = accessRequest.EntraIdObjectId;
        entity.FunctionEntityId = accessRequest.FunctionId;
        entity.CountryEntityId = accessRequest.CountryId;
        entity.BusinessProfileEntityId = accessRequest.BusinessProfileId;
        entity.UpdatedAt = accessRequest.UpdatedAt;
        entity.UpdatedBy = accessRequest.UpdatedBy;

        if (accessRequest.Projects.Count > 0)
        {
            entity.Projects = [];
            foreach (var item in accessRequest.Projects)
            {
                entity.Projects.Add(new()
                {
                    SapCode = item.SapCode,
                    ProjectTitle = item.ProjectTitle,
                });
            }
        }

        entity.DomainEvents = accessRequest.DomainEvents.ToList();
    }

    public static CountryAdminEntity MapCountryAdminToEntity(CountryAdmin countryAdmin)
    {
        return new CountryAdminEntity
        {
            Id = countryAdmin.Id,
            CountryId = countryAdmin.CountryId,
            UserId = countryAdmin.UserId,
            IsActive = true,
            CreatedAt = countryAdmin.CreatedAt,
            CreatedBy = countryAdmin.CreatedBy,
            UpdatedAt = countryAdmin.UpdatedAt,
            UpdatedBy = countryAdmin.UpdatedBy
        };
    }
}
