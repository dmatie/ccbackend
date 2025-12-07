namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public class AccessRequestDocumentEntity : BaseEntityConfiguration
{
    public Guid AccessRequestId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;

    public AccessRequestEntity? AccessRequest { get; set; }
}
