using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementA2NewParam
{
    public required string ReimbursementPurpose { get; set; }
    public required string ClaimantName { get; set; }
    public required string ClaimantAddress { get; set; }
    public required string ClaimantBankName { get; set; }
    public required string ClaimantBankAddress { get; set; }
    public required string ClaimantAccountNumber { get; set; }
    public required string ClaimantSwiftCode { get; set; }
    public required Money Amount { get; set; }
    public required DateTime ExpenseDate { get; set; }
    public required string ExpenseDescription { get; set; }
    public string? SupportingDocuments { get; set; }
    public string? SpecialInstructions { get; set; }
    public required string CreatedBy { get; set; }
}
