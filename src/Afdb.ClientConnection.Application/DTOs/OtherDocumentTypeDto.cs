namespace Afdb.ClientConnection.Application.DTOs;

public sealed class OtherDocumentTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameFr { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
}
