using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetClaimByIdQuery : IRequest<ClaimDto>
{
    public Guid ClaimId { get; init; }
}
