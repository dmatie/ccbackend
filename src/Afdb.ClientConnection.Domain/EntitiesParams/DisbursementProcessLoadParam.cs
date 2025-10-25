using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementProcessLoadParam : CommonLoadParam
{
    public required Guid DisbursementId { get; set; }
    public required DisbursementStatus Status { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    public required string Comment { get; set; }
    public required DateTime ProcessedAt { get; set; }
}
