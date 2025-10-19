using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed record CreateDisbursementCommand : IRequest<CreateDisbursementResponse>
{
    public string SapCodeProject { get; init; } = string.Empty;
    public string LoanGrantNumber { get; init; } = string.Empty;
    public Guid DisbursementTypeId { get; init; }

    public CreateDisbursementA1Command? DisbursementA1 { get; init; }
    public CreateDisbursementA2Command? DisbursementA2 { get; init; }
    public CreateDisbursementA3Command? DisbursementA3 { get; init; }
    public CreateDisbursementB1Command? DisbursementB1 { get; init; }
}

public sealed record CreateDisbursementA1Command
{
    public string PaymentPurpose { get; init; } = string.Empty;
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryAddress { get; init; } = string.Empty;
    public string BeneficiaryBankName { get; init; } = string.Empty;
    public string BeneficiaryBankAddress { get; init; } = string.Empty;
    public string BeneficiaryAccountNumber { get; init; } = string.Empty;
    public string BeneficiarySwiftCode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public string? IntermediaryBankName { get; init; }
    public string? IntermediaryBankSwiftCode { get; init; }
    public string? SpecialInstructions { get; init; }
}

public sealed record CreateDisbursementA2Command
{
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

public sealed record CreateDisbursementA3Command
{
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

public sealed record CreateDisbursementB1Command
{
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

public sealed record CreateDisbursementResponse
{
    public DisbursementDto Disbursement { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
