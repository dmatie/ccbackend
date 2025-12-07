namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record AccessRequestDocumentLoadParam : CommonLoadParam
{
    public Guid AccessRequestId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
}
