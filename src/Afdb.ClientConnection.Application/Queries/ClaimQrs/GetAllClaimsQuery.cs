using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetAllClaimsQuery : IRequest<GetAllClaimsResponse>
{
    public ClaimStatus? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

}

public sealed record GetAllClaimsResponse
{
    public List<ClaimDto> Claims { get; init; } =default!;
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}
