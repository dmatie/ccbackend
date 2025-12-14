namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record OtherDocumentFileLoadParam : CommonLoadParam
{
    public Guid OtherDocumentId { get; init; }
    public string FileName { get; init; } = default!;
    public long FileSize { get; init; }
    public string ContentType { get; init; } = default!;
    public DateTime UploadedAt { get; init; }
    public string UploadedBy { get; init; } = default!;
}
