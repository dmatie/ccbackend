using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementA1LoadParam
{
    public required Guid Id { get; set; }
    public required Guid DisbursementId { get; set; }
    public required string PaymentPurpose { get; set; }
    public required string BeneficiaryName { get; set; }
    public required string BeneficiaryAddress { get; set; }
    public required string BeneficiaryBankName { get; set; }
    public required string BeneficiaryBankAddress { get; set; }
    public required string BeneficiaryAccountNumber { get; set; }
    public required string BeneficiarySwiftCode { get; set; }
    public required Money Amount { get; set; }
    public string? IntermediaryBankName { get; set; }
    public string? IntermediaryBankSwiftCode { get; set; }
    public string? SpecialInstructions { get; set; }
    public required string CreatedBy { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
