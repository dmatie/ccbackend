using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed record AddOtherDocumentFileCommand : IRequest<AddOtherDocumentFileResponse>
{
    public Guid OtherDocumentId { get; init; }
    public IFormFileCollection Files { get; init; } = null!;
}

public sealed record AddOtherDocumentFileResponse
{
    public string Message { get; set; } = string.Empty;
    public int FilesUploaded { get; set; }
}
