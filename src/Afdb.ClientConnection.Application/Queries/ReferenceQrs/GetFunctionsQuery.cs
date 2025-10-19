using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetFunctionsQuery : IRequest<GetFunctionsResponse>
{

}

public sealed record GetFunctionsResponse
{
    public List<FunctionDto> Functions { get; init; } = new();
    public int TotalCount { get; init; }
}
