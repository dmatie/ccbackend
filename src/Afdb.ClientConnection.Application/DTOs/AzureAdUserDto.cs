namespace Afdb.ClientConnection.Application.DTOs;

public sealed record AzureAdUserDto
{
    public string Id { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
}

public sealed record SearchAzureAdUsersResponse
{
    public List<AzureAdUserDto> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
