namespace Afdb.ClientConnection.Application.DTOs;

public sealed record UserWithCountriesDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string OrganizationName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public string? EntraIdObjectId { get; init; }
    public List<CountryDto> Countries { get; init; } = [];
}
