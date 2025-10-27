using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed record ReSubmitDisbursementCommand : IRequest<ReSubmitDisbursementResponse>
{
    public Guid DisbursementId { get; init; }
    public string Comment { get; init; } = string.Empty;
    public List<IFormFile>? AdditionalDocuments { get; init; }
}

public sealed record ReSubmitDisbursementResponse
{
    public DisbursementDto Disbursement { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
