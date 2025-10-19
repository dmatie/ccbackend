using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
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

    public DisbursementA2(DisbursementA2NewParam newParam)
    {
        if (string.IsNullOrWhiteSpace(newParam.ReimbursementPurpose))
            throw new ArgumentException("ReimbursementPurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.ClaimantName))
            throw new ArgumentException("ClaimantName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.ClaimantAddress))
            throw new ArgumentException("ClaimantAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.ClaimantBankName))
            throw new ArgumentException("ClaimantBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.ClaimantBankAddress))
            throw new ArgumentException("ClaimantBankAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.ClaimantAccountNumber))
            throw new ArgumentException("ClaimantAccountNumber cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.ClaimantSwiftCode))
            throw new ArgumentException("ClaimantSwiftCode cannot be empty");

        if (newParam.Amount == null || newParam.Amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(newParam.ExpenseDescription))
            throw new ArgumentException("ExpenseDescription cannot be empty");

        ReimbursementPurpose = newParam.ReimbursementPurpose;
        ClaimantName = newParam.ClaimantName;
        ClaimantAddress = newParam.ClaimantAddress;
        ClaimantBankName = newParam.ClaimantBankName;
        ClaimantBankAddress = newParam.ClaimantBankAddress;
        ClaimantAccountNumber = newParam.ClaimantAccountNumber;
        ClaimantSwiftCode = newParam.ClaimantSwiftCode;
        Amount = newParam.Amount;
        ExpenseDate = newParam.ExpenseDate;
        ExpenseDescription = newParam.ExpenseDescription;
        SupportingDocuments = newParam.SupportingDocuments;
        SpecialInstructions = newParam.SpecialInstructions;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementA2(DisbursementA2LoadParam loadParam)
    {
        Id = loadParam.Id;
        DisbursementId = loadParam.DisbursementId;
        ReimbursementPurpose = loadParam.ReimbursementPurpose;
        ClaimantName = loadParam.ClaimantName;
        ClaimantAddress = loadParam.ClaimantAddress;
        ClaimantBankName = loadParam.ClaimantBankName;
        ClaimantBankAddress = loadParam.ClaimantBankAddress;
        ClaimantAccountNumber = loadParam.ClaimantAccountNumber;
        ClaimantSwiftCode = loadParam.ClaimantSwiftCode;
        Amount = loadParam.Amount;
        ExpenseDate = loadParam.ExpenseDate;
        ExpenseDescription = loadParam.ExpenseDescription;
        SupportingDocuments = loadParam.SupportingDocuments;
        SpecialInstructions = loadParam.SpecialInstructions;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
        UpdatedBy = loadParam.UpdatedBy;
        UpdatedAt = loadParam.UpdatedAt;
    }

    internal void SetDisbursementId(Guid disbursementId)
    {
        if (disbursementId == Guid.Empty)
            throw new ArgumentException("DisbursementId must be a valid GUID");

        DisbursementId = disbursementId;
    }
}
