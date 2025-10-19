namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementB1Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string GuaranteePurpose { get; init; } = string.Empty;
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryAddress { get; init; } = string.Empty;
    public string BeneficiaryBankName { get; init; } = string.Empty;
    public string BeneficiaryBankAddress { get; init; } = string.Empty;
    public decimal GuaranteeAmount { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public DateTime ValidityStartDate { get; init; }
    public DateTime ValidityEndDate { get; init; }
    public string GuaranteeTermsAndConditions { get; init; } = string.Empty;
    public string? SpecialInstructions { get; init; }
}
