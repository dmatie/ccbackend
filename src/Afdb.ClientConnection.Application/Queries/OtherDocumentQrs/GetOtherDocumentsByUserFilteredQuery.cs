using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed record GetOtherDocumentsByUserFilteredQuery : IRequest<GetOtherDocumentsByUserFilteredResponse>
{
    public OtherDocumentStatus? Status { get; init; }
    public Guid? OtherDocumentTypeId { get; init; }
    public string? SAPCode { get; init; }
    public string? Year { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public sealed record GetOtherDocumentsByUserFilteredResponse
{
    public List<OtherDocumentDto> OtherDocuments { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}
