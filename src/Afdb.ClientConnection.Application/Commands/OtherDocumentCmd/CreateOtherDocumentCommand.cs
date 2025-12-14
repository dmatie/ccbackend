using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed record CreateOtherDocumentCommand : IRequest<CreateOtherDocumentResponse>
{
    public Guid OtherDocumentTypeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Year { get; init; } = string.Empty;
    public string SAPCode { get; init; } = string.Empty;
    public string LoanNumber { get; init; } = string.Empty;
    public IFormFileCollection? Files { get; init; }
}

public sealed record CreateOtherDocumentResponse
{
    public OtherDocumentDto OtherDocument { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
