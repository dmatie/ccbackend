namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class AccessRequestDocumentNewParam
{
    public Guid AccessRequestId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
}
