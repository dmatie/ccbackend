using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetAccessRequestQuery : IRequest<GetAccessRequestResponse>
{
    public Guid Id { get; set; }
}

public sealed record GetAccessRequestResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
}