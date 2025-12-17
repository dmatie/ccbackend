using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class AddDisbursementDocumentsCommand : IRequest<AddDisbursementDocumentsResponse>
{
    public Guid DisbursementId { get; set; }
    public List<IFormFile> Documents { get; set; } = [];
}

public sealed class AddDisbursementDocumentsResponse
{
    public string Message { get; set; } = string.Empty;
    public int DocumentsAdded { get; set; }
}
