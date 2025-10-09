using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimLoadParam
{
    public Guid Id { get; init; }
    public Guid ClaimTypeId { get; init; }
    public Guid CountryId { get; init; }
    public Guid UserId { get; init; }
    public DateTime? ClosedAt { get; init; }
    public string Comment { get; init; }= default!;
    public User? User { get; init; } = default!;
    public ClaimType? ClaimType { get; init; } = default!;
    public Country? Country { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = default!;
    public string? UpdatedBy { get; init; } = default;
    public List<ClaimProcess> Processes { get; init; } = [];
}
