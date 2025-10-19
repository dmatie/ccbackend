namespace Afdb.ClientConnection.Domain.EntitiesParams;

public record CommonLoadParam
{
    public required Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = default!;
    public string? UpdatedBy { get; init; } = default;
}
