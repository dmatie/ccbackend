using MediatR;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed record SubmitOtherDocumentCommand : IRequest<SubmitOtherDocumentResponse>
{
    public Guid OtherDocumentId { get; init; }
}

public sealed record SubmitOtherDocumentResponse
{
    public string Message { get; set; } = string.Empty;
}
