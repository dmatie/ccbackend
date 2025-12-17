using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal partial class EntityMappings
{
    public static OtherDocumentEntity MapOtherDocumentToEntity(OtherDocument source)
    {
        var entity = new OtherDocumentEntity
        {
            Id = source.Id,
            OtherDocumentTypeId = source.OtherDocumentTypeId,
            Name = source.Name,
            Year = source.Year,
            UserId = source.UserId,
            Status = source.Status,
            SAPCode = source.SAPCode,
            LoanNumber = source.LoanNumber,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy,
            UpdatedAt = source.UpdatedAt,
            UpdatedBy = source.UpdatedBy
        };

        if (source.Files.Count > 0)
        {
            entity.Files = [];
            foreach (var file in source.Files)
            {
                entity.Files.Add(MapOtherDocumentFileToEntity(file));
            }
        }

        entity.DomainEvents = source.DomainEvents.ToList();

        return entity;
    }

    public static void UpdateOtherDocumentEntityFromDomain(OtherDocumentEntity entity, OtherDocument source)
    {
        entity.Name = source.Name;
        entity.Year = source.Year;
        entity.Status = source.Status;
        entity.SAPCode = source.SAPCode;
        entity.LoanNumber = source.LoanNumber;
        entity.UpdatedAt = source.UpdatedAt;
        entity.UpdatedBy = source.UpdatedBy;

        if (source.Files.Count > 0)
        {
            var existingIds = entity.Files.Select(f => f.Id).ToHashSet();
            var filesToAdd = source.Files.Where(file => !existingIds.Contains(file.Id)).ToList();

            foreach (var file in filesToAdd)
            {
                entity.Files.Add(MapOtherDocumentFileToEntity(file));
            }
        }

        entity.DomainEvents = source.DomainEvents.ToList();
    }

    public static OtherDocumentFileEntity MapOtherDocumentFileToEntity(OtherDocumentFile source)
    {
        var entity = new OtherDocumentFileEntity
        {
            OtherDocumentId = source.OtherDocumentId,
            FileName = source.FileName,
            FileSize = source.FileSize,
            ContentType = source.ContentType,
            UploadedAt = source.UploadedAt,
            UploadedBy = source.UploadedBy,
            CreatedAt = source.CreatedAt,
            CreatedBy = source.CreatedBy,
            UpdatedAt = source.UpdatedAt,
            UpdatedBy = source.UpdatedBy
        };

        return entity;
    }
}
