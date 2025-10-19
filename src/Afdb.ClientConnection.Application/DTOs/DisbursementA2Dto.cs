namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementA2Dto
{
    public Guid Id { get; init; }
    public Guid DisbursementId { get; init; }
    public string ReimbursementPurpose { get; init; } = string.Empty;
    public string ClaimantName { get; init; } = string.Empty;
    public string ClaimantAddress { get; init; } = string.Empty;
    public string ClaimantBankName { get; init; } = string.Empty;
    public string ClaimantBankAddress { get; init; } = string.Empty;
    public string ClaimantAccountNumber { get; init; } = string.Empty;
    public string ClaimantSwiftCode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public DateTime ExpenseDate { get; init; }
    public string ExpenseDescription { get; init; } = string.Empty;
    public string? SupportingDocuments { get; init; }
    public string? SpecialInstructions { get; init; }
}
