namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementA3Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string AdvancePurpose { get; init; } = string.Empty;
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientAddress { get; init; } = string.Empty;
    public string RecipientBankName { get; init; } = string.Empty;
    public string RecipientBankAddress { get; init; } = string.Empty;
    public string RecipientAccountNumber { get; init; } = string.Empty;
    public string RecipientSwiftCode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public DateTime ExpectedUsageDate { get; init; }
    public string JustificationForAdvance { get; init; } = string.Empty;
    public string? RepaymentTerms { get; init; }
    public string? SpecialInstructions { get; init; }
}
