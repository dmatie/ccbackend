using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class AccessRequestDocument : BaseEntity
{
    public Guid AccessRequestId { get; private set; }
    public string FileName { get; private set; }
    public string DocumentUrl { get; private set; }

    public AccessRequest? AccessRequest { get; private set; }

    private AccessRequestDocument() { }

    public AccessRequestDocument(AccessRequestDocumentNewParam newParam)
    {
        if (newParam == null)
            throw new ArgumentNullException(nameof(newParam));
        if (newParam.AccessRequestId == Guid.Empty)
            throw new ArgumentException("AccessRequestId must be a valid GUID");
        if (string.IsNullOrWhiteSpace(newParam.FileName))
            throw new ArgumentException("FileName cannot be empty");
        if (string.IsNullOrWhiteSpace(newParam.DocumentUrl))
            throw new ArgumentException("DocumentUrl cannot be empty");

        AccessRequestId = newParam.AccessRequestId;
        FileName = newParam.FileName;
        DocumentUrl = newParam.DocumentUrl;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = newParam.CreatedBy;
    }

    public AccessRequestDocument(AccessRequestDocumentLoadParam loadParam)
    {
        if (loadParam.AccessRequestId == Guid.Empty)
            throw new ArgumentException("AccessRequestId must be a valid GUID");

        if (string.IsNullOrWhiteSpace(loadParam.FileName))
            throw new ArgumentException("FileName cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.DocumentUrl))
            throw new ArgumentException("DocumentUrl cannot be empty");

        Id = loadParam.Id;
        AccessRequestId = loadParam.AccessRequestId;
        FileName = loadParam.FileName;
        DocumentUrl = loadParam.DocumentUrl;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
    }
}
