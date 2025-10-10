using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetAllClaimsQuery : IRequest<IEnumerable<ClaimDto>>
{
    public ClaimStaus? Status { get; init; }
}
