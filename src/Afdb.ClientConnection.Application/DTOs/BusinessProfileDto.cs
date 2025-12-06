
namespace Afdb.ClientConnection.Application.DTOs;

public sealed record BusinessProfileDto
{
    public Guid Id { get; init;}
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string NameFr { get; init; } = default!;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
}
