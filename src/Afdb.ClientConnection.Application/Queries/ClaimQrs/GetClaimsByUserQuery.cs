using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetClaimsByUserQuery : IRequest<GetClaimsByUserResponse>
{
    public ClaimStatus? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

}

public sealed record GetClaimsByUserResponse 
{
    public List<ClaimDto> Claims { get; init; } = default!;
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}
