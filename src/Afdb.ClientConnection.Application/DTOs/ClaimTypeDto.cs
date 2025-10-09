namespace Afdb.ClientConnection.Application.DTOs;

public sealed record ClaimTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
