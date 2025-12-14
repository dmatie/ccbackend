using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class OtherDocumentFile : BaseEntity
{
    public Guid OtherDocumentId { get; private set; }
    public string FileName { get; private set; }
    public long FileSize { get; private set; }
    public string ContentType { get; private set; }
    public DateTime UploadedAt { get; private set; }
    public string UploadedBy { get; private set; }

    private OtherDocumentFile() { }

    public OtherDocumentFile(OtherDocumentFileNewParam newParam)
    {
        if (newParam.OtherDocumentId == Guid.Empty)
            throw new ArgumentException("OtherDocumentId must be a valid GUID", nameof(newParam.OtherDocumentId));

        if (string.IsNullOrWhiteSpace(newParam.FileName))
            throw new ArgumentException("FileName cannot be empty", nameof(newParam.FileName));

        if (string.IsNullOrWhiteSpace(newParam.ContentType))
            throw new ArgumentException("ContentType cannot be empty", nameof(newParam.ContentType));

        if (newParam.FileSize <= 0)
            throw new ArgumentException("FileSize must be greater than 0", nameof(newParam.FileSize));

        OtherDocumentId = newParam.OtherDocumentId;
        FileName = newParam.FileName;
        FileSize = newParam.FileSize;
        ContentType = newParam.ContentType;
        UploadedAt = newParam.UploadedAt;
        UploadedBy = newParam.UploadedBy;
        CreatedBy = newParam.CreatedBy;
    }

    public OtherDocumentFile(OtherDocumentFileLoadParam loadParam)
    {
        Id = loadParam.Id;
        OtherDocumentId = loadParam.OtherDocumentId;
        FileName = loadParam.FileName;
        FileSize = loadParam.FileSize;
        ContentType = loadParam.ContentType;
        UploadedAt = loadParam.UploadedAt;
        UploadedBy = loadParam.UploadedBy;
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }
}
