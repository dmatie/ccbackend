namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public class OtherDocumentFileEntity : BaseEntityConfiguration
{
    public Guid OtherDocumentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string UploadedBy { get; set; } = string.Empty;

    public OtherDocumentEntity? OtherDocument { get; set; }
}
