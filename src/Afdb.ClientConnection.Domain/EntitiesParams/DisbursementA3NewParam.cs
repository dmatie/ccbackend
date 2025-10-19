using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed class DisbursementA3NewParam
{
    public required string AdvancePurpose { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientAddress { get; set; }
    public required string RecipientBankName { get; set; }
    public required string RecipientBankAddress { get; set; }
    public required string RecipientAccountNumber { get; set; }
    public required string RecipientSwiftCode { get; set; }
    public required Money Amount { get; set; }
    public required DateTime ExpectedUsageDate { get; set; }
    public required string JustificationForAdvance { get; set; }
    public string? RepaymentTerms { get; set; }
    public string? SpecialInstructions { get; set; }
    public required string CreatedBy { get; set; }
}
