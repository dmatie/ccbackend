using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementProcessLoadParam
{
    public required Guid Id { get; set; }
    public required Guid DisbursementId { get; set; }
    public required DisbursementStatus Status { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    public required string Comment { get; set; }
    public required DateTime ProcessedAt { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public  string? UpdatedBy { get; set; }
    public  DateTime? UpdatedAt { get; set; }
}
