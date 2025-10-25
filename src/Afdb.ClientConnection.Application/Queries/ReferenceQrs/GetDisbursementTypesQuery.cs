using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetDisbursementTypesQuery : IRequest<GetDisbursementTypesResponse>
{
}

public sealed record GetDisbursementTypesResponse
{
    public List<DisbursementTypeDto> DisbursementTypes { get; init; } = new();
    public int TotalCount { get; init; }
}

