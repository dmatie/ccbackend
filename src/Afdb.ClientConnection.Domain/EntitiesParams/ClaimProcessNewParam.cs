using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimProcessNewParam 
{
    public required Guid ClaimId { get; init; }
    public required Guid UserId { get; init; }
    public required ClaimStatus Status { get; init; }
    public required User User { get; init; } = default!;
    public required string Comment { get; init; } = default!;
    public string CreatedBy { get; init; } = default!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
