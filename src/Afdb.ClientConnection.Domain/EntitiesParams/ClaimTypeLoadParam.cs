using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimTypeLoadParam
{
    public Guid Id { get; init; }
    public string Name { get; init; }= default!;
    public string NameFr { get; init; }= default!;
    public string Description { get; init; }= default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; } = default!;
    public string CreatedBy { get; init; } = default!;
    public string? UpdatedBy { get; init; } = default;

}
