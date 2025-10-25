using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementDocument : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string FileName { get; private set; }
    public string DocumentUrl { get; private set; }

    public Disbursement? Disbursement { get; private set; }

    private DisbursementDocument() { }

    public DisbursementDocument(DisbursementDocumentNewParam newParam)
    {
        if (newParam == null)
            throw new ArgumentNullException(nameof(newParam));
        if (newParam.DisbursementId == Guid.Empty)
            throw new ArgumentException("DisbursementId must be a valid GUID");
        if (string.IsNullOrWhiteSpace(newParam.FileName))
            throw new ArgumentException("FileName cannot be empty");
        if (string.IsNullOrWhiteSpace(newParam.DocumentUrl))
            throw new ArgumentException("DocumentUrl cannot be empty");

        DisbursementId = newParam.DisbursementId;
        FileName = newParam.FileName;
        DocumentUrl = newParam.DocumentUrl;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementDocument(DisbursementDocumentLoadParam loadParam)
    {
        if (loadParam.DisbursementId == Guid.Empty)
            throw new ArgumentException("DisbursementId must be a valid GUID");

        if (string.IsNullOrWhiteSpace(loadParam.FileName))
            throw new ArgumentException("FileName cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.DocumentUrl))
            throw new ArgumentException("FileUrl cannot be empty");

        DisbursementId = loadParam.DisbursementId;
        FileName = loadParam.FileName;
        DocumentUrl = loadParam.DocumentUrl;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
    }
}
