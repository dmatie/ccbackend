using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementA2 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string ReimbursementPurpose { get; private set; }
    public string ClaimantName { get; private set; }
    public string ClaimantAddress { get; private set; }
    public string ClaimantBankName { get; private set; }
    public string ClaimantBankAddress { get; private set; }
    public string ClaimantAccountNumber { get; private set; }
    public string ClaimantSwiftCode { get; private set; }
    public Money Amount { get; private set; }
    public DateTime ExpenseDate { get; private set; }
    public string ExpenseDescription { get; private set; }
    public string? SupportingDocuments { get; private set; }
    public string? SpecialInstructions { get; private set; }

    public Disbursement? Disbursement { get; private set; }

    private DisbursementA2() { }

    public DisbursementA2(
        string reimbursementPurpose,
        string claimantName,
        string claimantAddress,
        string claimantBankName,
        string claimantBankAddress,
        string claimantAccountNumber,
        string claimantSwiftCode,
        Money amount,
        DateTime expenseDate,
        string expenseDescription,
        string? supportingDocuments,
        string? specialInstructions,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(reimbursementPurpose))
            throw new ArgumentException("ReimbursementPurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(claimantName))
            throw new ArgumentException("ClaimantName cannot be empty");

        if (string.IsNullOrWhiteSpace(claimantAddress))
            throw new ArgumentException("ClaimantAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(claimantBankName))
            throw new ArgumentException("ClaimantBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(claimantBankAddress))
            throw new ArgumentException("ClaimantBankAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(claimantAccountNumber))
            throw new ArgumentException("ClaimantAccountNumber cannot be empty");

        if (string.IsNullOrWhiteSpace(claimantSwiftCode))
            throw new ArgumentException("ClaimantSwiftCode cannot be empty");

        if (amount == null || amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(expenseDescription))
            throw new ArgumentException("ExpenseDescription cannot be empty");

        ReimbursementPurpose = reimbursementPurpose;
        ClaimantName = claimantName;
        ClaimantAddress = claimantAddress;
        ClaimantBankName = claimantBankName;
        ClaimantBankAddress = claimantBankAddress;
        ClaimantAccountNumber = claimantAccountNumber;
        ClaimantSwiftCode = claimantSwiftCode;
        Amount = amount;
        ExpenseDate = expenseDate;
        ExpenseDescription = expenseDescription;
        SupportingDocuments = supportingDocuments;
        SpecialInstructions = specialInstructions;
        CreatedBy = createdBy;
    }

    internal void SetDisbursementId(Guid disbursementId)
    {
        if (disbursementId == Guid.Empty)
            throw new ArgumentException("DisbursementId must be a valid GUID");

        DisbursementId = disbursementId;
    }
}
