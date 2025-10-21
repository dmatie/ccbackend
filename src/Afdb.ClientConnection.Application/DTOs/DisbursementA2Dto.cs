namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementA2Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string ReimbursementPurpose { get; init; } = string.Empty;
    public string Contractor { get; init; } = string.Empty;
    
    public string GoodDescription { get; init; } = string.Empty;
    public Guid GoodOriginCountryId { get; init; }
    public CountryDto? GoodOriginCountry { get; init; }
    
    public string ContractBorrowerReference { get; init; } = string.Empty;
    public string ContractAfDBReference { get; init; } = string.Empty;
    public string ContractValue { get; init; } = string.Empty;
    public string ContractBankShare { get; init; } = string.Empty;
    public decimal ContractAmountPreviouslyPaid { get; init; }
    
    public string InvoiceRef { get; init; } = string.Empty;
    public DateTime InvoiceDate { get; init; }
    public decimal InvoiceAmount { get; init; }
    
    public DateTime PaymentDateOfPayment { get; init; }
    public decimal PaymentAmountWithdrawn { get; init; }
    public string PaymentEvidenceOfPayment { get; init; } = string.Empty;
}
