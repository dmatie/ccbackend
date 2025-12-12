using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetSignedFormUploadedQuery : IRequest<FileUploadedDto>
{
    public Guid Id { get; init; }
}
