namespace Afdb.ClientConnection.Application.DTOs;

public sealed class OtherDocumentFileDto
{
    public Guid Id { get; init; }
    public Guid OtherDocumentId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public string ContentType { get; init; } = string.Empty;
    public DateTime UploadedAt { get; init; }
    public string UploadedBy { get; init; } = string.Empty;
}
