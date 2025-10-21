namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementA1Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string PaymentPurpose { get; init; } = string.Empty;
    
    public string BeneficiaryBpNumber { get; init; } = string.Empty;
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryContactPerson { get; init; } = string.Empty;
    public string BeneficiaryAddress { get; init; } = string.Empty;
    public Guid BeneficiaryCountryId { get; init; }
    public string BeneficiaryEmail { get; init; } = string.Empty;
    public CountryDto? BeneficiaryCountry { get; init; }
    
    public string CorrespondentBankName { get; init; } = string.Empty;
    public string CorrespondentBankAddress { get; init; } = string.Empty;
    public Guid CorrespondentBankCountryId { get; init; }
    public string CorrespondentAccountNumber { get; init; } = string.Empty;
    public string CorrespondentBankSwiftCode { get; init; } = string.Empty;
    public CountryDto? CorrespondentBankCountry { get; init; }
    
    public decimal Amount { get; init; }
    
    public string SignatoryName { get; init; } = string.Empty;
    public string SignatoryContactPerson { get; init; } = string.Empty;
    public string SignatoryAddress { get; init; } = string.Empty;
    public Guid SignatoryCountryId { get; init; }
    public string SignatoryEmail { get; init; } = string.Empty;
    public string SignatoryPhone { get; init; } = string.Empty;
    public string SignatoryTitle { get; init; } = string.Empty;
    public CountryDto? SignatoryCountry { get; init; }
}
