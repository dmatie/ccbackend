using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed record BackToClientDisbursementCommand : IRequest<BackToClientDisbursementResponse>
{
    public Guid DisbursementId { get; init; }
    public string Comment { get; init; } = string.Empty;
    public List<IFormFile>? AdditionalDocuments { get; init; }
}

public sealed record BackToClientDisbursementResponse
{
    public DisbursementDto Disbursement { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
