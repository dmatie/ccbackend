namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementA3Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string PeriodForUtilization { get; init; } = string.Empty;
    public int ItemNumber { get; init; }
    
    public string GoodDescription { get; init; } = string.Empty;
    public Guid GoodOriginCountryId { get; init; }
    public int GoodQuantity { get; init; }
    public CountryDto? GoodOriginCountry { get; init; }
    
    public decimal AnnualBudget { get; init; }
    public decimal BankShare { get; init; }
    public decimal AdvanceRequested { get; init; }
    
    public DateTime DateOfApproval { get; init; }
}
