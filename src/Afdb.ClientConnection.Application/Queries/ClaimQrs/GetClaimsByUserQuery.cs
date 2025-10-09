using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetClaimsByUserQuery : IRequest<IEnumerable<ClaimDto>>
{
    public Guid UserId { get; init; }
    public ClaimStaus? Status { get; init; }
}
