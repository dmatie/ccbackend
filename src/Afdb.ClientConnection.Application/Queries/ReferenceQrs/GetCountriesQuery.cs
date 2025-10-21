using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetCountriesQuery : IRequest<GetCountriesResponse>
{

}

public sealed record GetCountriesResponse
{
    public List<CountryDto> Countries { get; init; } = new();
    public int TotalCount { get; init; }
}
