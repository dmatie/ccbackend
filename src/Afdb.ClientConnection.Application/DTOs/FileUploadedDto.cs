namespace Afdb.ClientConnection.Application.DTOs;

public sealed record FileUploadedDto
{
    public string FileName { get; init; } = default!;
    public Stream FileContent { get; init; } = default!;
    public string ContentType { get; init; } = default!;
}
