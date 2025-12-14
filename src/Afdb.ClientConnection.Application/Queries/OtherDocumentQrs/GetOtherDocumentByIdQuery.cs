using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed record GetOtherDocumentByIdQuery : IRequest<OtherDocumentDto?>
{
    public Guid OtherDocumentId { get; init; }
}
