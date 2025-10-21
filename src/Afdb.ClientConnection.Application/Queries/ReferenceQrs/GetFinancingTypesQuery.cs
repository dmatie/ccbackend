using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetFinancingTypesQuery : IRequest<GetFinancingTypesResponse>
{

}

public sealed record GetFinancingTypesResponse
{
    public List<FinancingTypeDto> FinancingTypes { get; init; } = new();
    public int TotalCount { get; init; }
}
