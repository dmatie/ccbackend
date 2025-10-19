using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementNewParam
{
    public required string RequestNumber { get; set; }
    public required string SapCodeProject { get; set; }
    public required string LoanGrantNumber { get; set; }
    public required Guid DisbursementTypeId { get; set; }
    public DisbursementType? DisbursementType { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    public required string CreatedBy { get; set; }

    public DisbursementA1? DisbursementA1 { get; set; }
    public DisbursementA2? DisbursementA2 { get; set; }
    public DisbursementA3? DisbursementA3 { get; set; }
    public DisbursementB1? DisbursementB1 { get; set; }
}
