using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class DeleteDisbursementDocumentCommand : IRequest<DeleteDisbursementDocumentResponse>
{
    public Guid DisbursementId { get; set; }
    public Guid DocumentId { get; set; }
}

public sealed class DeleteDisbursementDocumentResponse
{
    public string Message { get; set; } = string.Empty;
}
