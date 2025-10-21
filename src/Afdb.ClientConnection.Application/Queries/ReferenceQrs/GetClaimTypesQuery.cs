using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetClaimTypesQuery : IRequest<GetClaimTypesResponse>
{

}

public sealed record GetClaimTypesResponse
{
    public List<ClaimTypeDto> ClaimTypes { get; init; } = new();
    public int TotalCount { get; init; }
}
