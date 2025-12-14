using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed record GetFirstFileQuery : IRequest<FileUploadedDto>
{
    public Guid OtherDocumentId { get; init; }
}
