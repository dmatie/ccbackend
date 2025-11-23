using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetApprovedAccessRequestsQuery : IRequest<PaginatedAccessRequestDto>
{
    public Guid? CountryId { get; init; }
    public string? ProjectCode { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
