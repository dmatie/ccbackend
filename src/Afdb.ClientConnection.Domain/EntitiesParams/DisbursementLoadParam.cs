using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementLoadParam : CommonLoadParam
{
    public required string RequestNumber { get; init; }
    public required string SapCodeProject { get; init; }
    public required string LoanGrantNumber { get; init; }
    public required Guid DisbursementTypeId { get; init; }
    public required Guid CurrencyId { get; init; }
    public DisbursementType? DisbursementType { get; init; }
    public required DisbursementStatus Status { get; init; }
    public required Guid CreatedByUserId { get; init; }
    public User? CreatedByUser { get; init; }
    public DateTime? SubmittedAt { get; init; }
    public DateTime? ProcessedAt { get; init; }
    public Guid? ProcessedByUserId { get; init; }
    public User? ProcessedByUser { get; init; }
    public Currency? Currency { get; init; } = default!;
    public DisbursementA1? DisbursementA1 { get; init; }
    public DisbursementA2? DisbursementA2 { get; init; }
    public DisbursementA3? DisbursementA3 { get; init; }
    public DisbursementB1? DisbursementB1 { get; init; }
    public List<DisbursementProcess>? Processes { get; init; }
    public List<DisbursementDocument>? Documents { get; init; }
}
