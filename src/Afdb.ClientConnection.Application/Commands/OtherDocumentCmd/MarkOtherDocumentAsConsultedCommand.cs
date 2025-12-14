using MediatR;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed record MarkOtherDocumentAsConsultedCommand : IRequest<MarkOtherDocumentAsConsultedResponse>
{
    public Guid OtherDocumentId { get; init; }
}

public sealed record MarkOtherDocumentAsConsultedResponse
{
    public string Message { get; set; } = string.Empty;
}
