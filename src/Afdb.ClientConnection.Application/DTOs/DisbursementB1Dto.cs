namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementB1Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    
    public string GuaranteeDetails { get; init; } = string.Empty;
    public string ConfirmingBank { get; init; } = string.Empty;
    
    public string IssuingBankName { get; init; } = string.Empty;
    public string IssuingBankAddress { get; init; } = string.Empty;
    public decimal GuaranteeAmount { get; init; }
    public DateTime ExpiryDate { get; init; }
    
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryBPNumber { get; init; } = string.Empty;
    public string BeneficiaryAFDBContract { get; init; } = string.Empty;
    public string BeneficiaryBankAddress { get; init; } = string.Empty;
    public string BeneficiaryCity { get; init; } = string.Empty;
    public Guid BeneficiaryCountryId { get; init; }
    public string GoodDescription { get; init; } = string.Empty;
    public string BeneficiaryLcContractRef { get; init; } = string.Empty;
    public CountryDto? BeneficiaryCountry { get; init; }
    
    public string ExecutingAgencyName { get; init; } = string.Empty;
    public string ExecutingAgencyContactPerson { get; init; } = string.Empty;
    public string ExecutingAgencyAddress { get; init; } = string.Empty;
    public string ExecutingAgencyCity { get; init; } = string.Empty;
    public Guid ExecutingAgencyCountryId { get; init; }
    public string ExecutingAgencyEmail { get; init; } = string.Empty;
    public string ExecutingAgencyPhone { get; init; } = string.Empty;
    public CountryDto? ExecutingAgencyCountry { get; init; }
}
