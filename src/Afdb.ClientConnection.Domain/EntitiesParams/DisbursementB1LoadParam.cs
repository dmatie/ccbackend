using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementB1LoadParam
{
    public required Guid Id { get; set; }
    public required Guid DisbursementId { get; set; }
    public required string GuaranteePurpose { get; set; }
    public required string BeneficiaryName { get; set; }
    public required string BeneficiaryAddress { get; set; }
    public required string BeneficiaryBankName { get; set; }
    public required string BeneficiaryBankAddress { get; set; }
    public required Money GuaranteeAmount { get; set; }
    public required DateTime ValidityStartDate { get; set; }
    public required DateTime ValidityEndDate { get; set; }
    public required string GuaranteeTermsAndConditions { get; set; }
    public string? SpecialInstructions { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
