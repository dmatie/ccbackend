namespace Afdb.ClientConnection.Domain.Entities;

public sealed class FileDownloaded
{
    public string FileName { get; init; } = default!;
    public Stream FileContent { get; init; } = default!;
    public string ContentType { get; init; } = default!;
}
