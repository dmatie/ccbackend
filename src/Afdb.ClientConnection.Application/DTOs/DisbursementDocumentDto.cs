namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementDocumentDto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public Stream FileContent { get; set; } = default!;
    public long FileSize { get; init; }
    public DateTime UploadedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
