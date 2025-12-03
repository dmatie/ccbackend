using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetApprovedAccessRequestsQuery : IRequest<GetApprovedAccessRequestsResponse>
{
    public Guid? CountryId { get; init; }
    public string? ProjectCode { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public sealed record GetApprovedAccessRequestsResponse
{
    public List<AccessRequestDto> AccessRequests { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}
