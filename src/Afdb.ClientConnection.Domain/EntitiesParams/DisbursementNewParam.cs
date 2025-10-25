using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementNewParam
{
    public required string RequestNumber { get; init; }
    public required string SapCodeProject { get; init; }
    public required string LoanGrantNumber { get; init; }
    public required Guid DisbursementTypeId { get; init; }
    public DisbursementType? DisbursementType { get; init; }
    public required Guid CreatedByUserId { get; init; }
    public required Guid CurrencyId { get; init; }
    public User? CreatedByUser { get; init; }
    public required string CreatedBy { get; init; }

    public DisbursementA1? DisbursementA1 { get; init; }
    public DisbursementA2? DisbursementA2 { get; init; }
    public DisbursementA3? DisbursementA3 { get; init; }
    public DisbursementB1? DisbursementB1 { get; init; }
}
