using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimProcessLoadParam
{
    public Guid Id { get; set; }
    public Guid ClaimId { get; init; }
    public Guid UserId { get; init; }
    public User? User { get; init; } = default!;
    public string Comment { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = default!;
    public string? UpdatedBy { get; init; } = default;
}
