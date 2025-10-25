namespace Afdb.ClientConnection.Application.DTOs;

public sealed record CurrencyDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Symbol { get; init; } = default!;
}
