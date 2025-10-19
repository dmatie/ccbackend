using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed record ApproveDisbursementCommand : IRequest<ApproveDisbursementResponse>
{
    public Guid DisbursementId { get; init; }
}

public sealed record ApproveDisbursementResponse
{
    public DisbursementDto Disbursement { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
