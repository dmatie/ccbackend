using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetAccessRequestByEmailQuery : IRequest<GetAccessRequestByEmailResponse>
{
    public string Email { get; init; } = string.Empty;
}

public sealed record GetAccessRequestByEmailResponse
{
    public AccessRequestDto AccessRequest { get; set; } = new();
}