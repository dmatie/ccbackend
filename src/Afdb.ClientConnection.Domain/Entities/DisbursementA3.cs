using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementA3 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string AdvancePurpose { get; private set; }
    public string RecipientName { get; private set; }
    public string RecipientAddress { get; private set; }
    public string RecipientBankName { get; private set; }
    public string RecipientBankAddress { get; private set; }
    public string RecipientAccountNumber { get; private set; }
    public string RecipientSwiftCode { get; private set; }
    public Money Amount { get; private set; }
    public DateTime ExpectedUsageDate { get; private set; }
    public string JustificationForAdvance { get; private set; }
    public string? RepaymentTerms { get; private set; }
    public string? SpecialInstructions { get; private set; }

    public Disbursement? Disbursement { get; private set; }

    private DisbursementA3() { }

    public DisbursementA3(
        string advancePurpose,
        string recipientName,
        string recipientAddress,
        string recipientBankName,
        string recipientBankAddress,
        string recipientAccountNumber,
        string recipientSwiftCode,
        Money amount,
        DateTime expectedUsageDate,
        string justificationForAdvance,
        string? repaymentTerms,
        string? specialInstructions,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(advancePurpose))
            throw new ArgumentException("AdvancePurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentException("RecipientName cannot be empty");

        if (string.IsNullOrWhiteSpace(recipientAddress))
            throw new ArgumentException("RecipientAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(recipientBankName))
            throw new ArgumentException("RecipientBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(recipientBankAddress))
            throw new ArgumentException("RecipientBankAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(recipientAccountNumber))
            throw new ArgumentException("RecipientAccountNumber cannot be empty");

        if (string.IsNullOrWhiteSpace(recipientSwiftCode))
            throw new ArgumentException("RecipientSwiftCode cannot be empty");

        if (amount == null || amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(justificationForAdvance))
            throw new ArgumentException("JustificationForAdvance cannot be empty");

        AdvancePurpose = advancePurpose;
        RecipientName = recipientName;
        RecipientAddress = recipientAddress;
        RecipientBankName = recipientBankName;
        RecipientBankAddress = recipientBankAddress;
        RecipientAccountNumber = recipientAccountNumber;
        RecipientSwiftCode = recipientSwiftCode;
        Amount = amount;
        ExpectedUsageDate = expectedUsageDate;
        JustificationForAdvance = justificationForAdvance;
        RepaymentTerms = repaymentTerms;
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
