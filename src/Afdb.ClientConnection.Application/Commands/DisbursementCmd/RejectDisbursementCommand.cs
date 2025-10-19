using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed record RejectDisbursementCommand : IRequest<RejectDisbursementResponse>
{
    public Guid DisbursementId { get; init; }
    public string Comment { get; init; } = string.Empty;
}

public sealed record RejectDisbursementResponse
{
    public DisbursementDto Disbursement { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
