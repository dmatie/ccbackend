using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementProcessLoadParam
{
    public required Guid Id { get; set; }
    public required Guid DisbursementId { get; set; }
    public required DisbursementStatus Status { get; set; }
    public required Guid ProcessedByUserId { get; set; }
    public User? ProcessedByUser { get; set; }
    public required string Comment { get; set; }
    public required DateTime ProcessedAt { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
}
