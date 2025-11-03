namespace Afdb.ClientConnection.Application.DTOs;

public sealed record AzureAdUserDetailsDto
{
    public string Id { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
}
