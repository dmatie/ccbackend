using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed record GetAccessRequestsQuery : IRequest<GetAccessRequestsResponse>
{
    public RequestStatus? Status { get; set; }
    public string? Email { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public sealed record GetAccessRequestsResponse
{
    public List<AccessRequestDto> AccessRequests { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}