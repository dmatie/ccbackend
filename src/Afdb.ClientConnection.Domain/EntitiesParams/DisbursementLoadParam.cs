using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementLoadParam
{
    public required Guid Id { get; set; }
    public required string RequestNumber { get; set; }
    public required string SapCodeProject { get; set; }
    public required string LoanGrantNumber { get; set; }
    public required Guid DisbursementTypeId { get; set; }
    public DisbursementType? DisbursementType { get; set; }
    public required DisbursementStatus Status { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessedByUserId { get; set; }
    public User? ProcessedByUser { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DisbursementA1? DisbursementA1 { get; set; }
    public DisbursementA2? DisbursementA2 { get; set; }
    public DisbursementA3? DisbursementA3 { get; set; }
    public DisbursementB1? DisbursementB1 { get; set; }

    public List<DisbursementProcess>? Processes { get; set; }
    public List<DisbursementDocument>? Documents { get; set; }
}
