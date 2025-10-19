using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetClaimByIdQuery : IRequest<GetClaimByIdResponse>
{
    public Guid ClaimId { get; init; }
}

public sealed record GetClaimByIdResponse
{
    public ClaimDto Claim { get; init; } = default!;
}
