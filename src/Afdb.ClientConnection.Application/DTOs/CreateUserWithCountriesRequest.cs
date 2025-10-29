namespace Afdb.ClientConnection.Application.DTOs;

public sealed record CreateUserWithCountriesRequest
{
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public List<Guid> CountryIds { get; init; } = [];
}
