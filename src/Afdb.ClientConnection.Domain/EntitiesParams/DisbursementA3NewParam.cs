namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA3NewParam
{
    public string PeriodForUtilization { get; init; } = default!;
    public int ItemNumber { get; init; }
    public string GoodDescription { get; init; } = default!;
    public Guid GoodOrginCountryId { get; init; }
    public int GoodQuantity { get; init; }
    public decimal AnnualBudget { get; init; }
    public decimal BankShare { get; init; }
    public decimal AdvanceRequested { get; init; }
    public DateTime DateOfApproval { get; init; }
}
