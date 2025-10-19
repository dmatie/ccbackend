
namespace Afdb.ClientConnection.Application.DTOs;

public sealed record FinancingTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
