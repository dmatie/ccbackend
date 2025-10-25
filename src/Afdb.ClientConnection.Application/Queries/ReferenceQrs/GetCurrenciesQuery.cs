using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed record GetCurrenciesQuery : IRequest<GetCurrenciesResponse>
{
}

public sealed record GetCurrenciesResponse
{
    public List<CurrencyDto> Currencies { get; init; } = new();
    public int TotalCount { get; init; }
}

