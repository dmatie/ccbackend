using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record CurrencyLoadParam :  CommonLoadParam
{
    public string Name { get; init; }= default!;
    public string Code { get; init; }= default!;
    public string? Symbol { get; init; }= default!;
}
