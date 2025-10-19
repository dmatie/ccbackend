using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetAccessRequestIsRejectedQuery : IRequest<bool>
{
    public string Email { get; init; }= string.Empty;
}
