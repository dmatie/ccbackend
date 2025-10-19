using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementDocument : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string FileName { get; private set; }
    public string FileUrl { get; private set; }
    public string ContentType { get; private set; }
    public long FileSize { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public Disbursement? Disbursement { get; private set; }

    private DisbursementDocument() { }

    public DisbursementDocument(Guid disbursementId, string fileName, string fileUrl, string contentType, long fileSize, string createdBy)
    {
        if (disbursementId == Guid.Empty)
            throw new ArgumentException("DisbursementId must be a valid GUID");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName cannot be empty");

        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException("FileUrl cannot be empty");

        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("ContentType cannot be empty");

        if (fileSize <= 0)
            throw new ArgumentException("FileSize must be greater than zero");

        DisbursementId = disbursementId;
        FileName = fileName;
        FileUrl = fileUrl;
        ContentType = contentType;
        FileSize = fileSize;
        UploadedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }
}
