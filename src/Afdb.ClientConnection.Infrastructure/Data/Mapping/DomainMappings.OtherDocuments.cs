using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static OtherDocumentType MapOtherDocumentTypeToDomain(OtherDocumentTypeEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var otherDocumentType = new OtherDocumentType(new OtherDocumentTypeLoadParam
        {
            Id = entity.Id,
            Name = entity.Name,
            NameFr = entity.NameFr,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });

        return otherDocumentType;
    }

    public static OtherDocument MapOtherDocumentToDomain(OtherDocumentEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var otherDocument = new OtherDocument(new OtherDocumentLoadParam
        {
            Id = entity.Id,
            OtherDocumentTypeId = entity.OtherDocumentTypeId,
            Name = entity.Name,
            Year = entity.Year,
            UserId = entity.UserId,
            Status = entity.Status,
            SAPCode = entity.SAPCode,
            LoanNumber = entity.LoanNumber,
            OtherDocumentType = entity.OtherDocumentType != null ? MapOtherDocumentTypeToDomain(entity.OtherDocumentType) : null,
            User = entity.User != null ? MapUser(entity.User) : null,
            Files = entity.Files?.Select(MapOtherDocumentFileToDomain).ToList() ?? [],
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });

        return otherDocument;
    }

    public static OtherDocumentFile MapOtherDocumentFileToDomain(OtherDocumentFileEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var file = new OtherDocumentFile(new OtherDocumentFileLoadParam
        {
            Id = entity.Id,
            OtherDocumentId = entity.OtherDocumentId,
            FileName = entity.FileName,
            FileSize = entity.FileSize,
            ContentType = entity.ContentType,
            UploadedAt = entity.UploadedAt,
            UploadedBy = entity.UploadedBy,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });

        return file;
    }
}
