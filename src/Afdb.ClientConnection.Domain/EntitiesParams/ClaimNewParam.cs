using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimNewParam
{
    public Guid ClaimTypeId { get; init; }
    public Guid CountryId { get; init; }
    public Guid UserId { get; init; }
    public DateTime? ClosedAt { get; init; }
    public string Comment { get; init; } = default!;
    public User User { get; init; } = default!;
    public Country Country { get; init; } = default!;
    public ClaimType ClaimType { get; init; } = default!;
    public string[] AssignTo { get; init; } = [];
    public string[] AssignCc { get; init; } = [];
}
