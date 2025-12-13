using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed record GetClaimsByUserFilteredQuery : IRequest<GetClaimsByUserFilteredResponse>
{
    public ClaimStatus? Status { get; init; }
    public Guid? ClaimTypeId { get; init; }
    public Guid? CountryId { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public sealed record GetClaimsByUserFilteredResponse
{
    public List<ClaimDto> Claims { get; init; } = default!;
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}
