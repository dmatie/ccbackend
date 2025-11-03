namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record AzureAdUserDetailsLoadParam
{
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
    public string? UserType { get; init; } = string.Empty;
}
