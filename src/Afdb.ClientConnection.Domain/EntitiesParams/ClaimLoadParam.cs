using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimLoadParam : CommonLoadParam
{
    public required Guid ClaimTypeId { get; init; }
    public required Guid CountryId { get; init; }
    public required Guid UserId { get; init; }
    public DateTime? ClosedAt { get; init; }
    public required string Comment { get; init; }= default!;
    public required ClaimStatus Status { get; init; }= default!;
    public User? User { get; init; } = default!;
    public ClaimType? ClaimType { get; init; } = default!;
    public Country? Country { get; init; } = default!;
    public List<ClaimProcess> Processes { get; init; } = [];
}
