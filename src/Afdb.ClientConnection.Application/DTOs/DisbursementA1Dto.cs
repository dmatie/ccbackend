namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementA1Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string PaymentPurpose { get; init; } = string.Empty;
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryAddress { get; init; } = string.Empty;
    public string BeneficiaryBankName { get; init; } = string.Empty;
    public string BeneficiaryBankAddress { get; init; } = string.Empty;
    public string BeneficiaryAccountNumber { get; init; } = string.Empty;
    public string BeneficiarySwiftCode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public string? IntermediaryBankName { get; init; }
    public string? IntermediaryBankSwiftCode { get; init; }
    public string? SpecialInstructions { get; init; }
}
