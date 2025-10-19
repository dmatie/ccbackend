using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
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

    public DisbursementA3(DisbursementA3NewParam newParam)
    {
        if (string.IsNullOrWhiteSpace(newParam.AdvancePurpose))
            throw new ArgumentException("AdvancePurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.RecipientName))
            throw new ArgumentException("RecipientName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.RecipientAddress))
            throw new ArgumentException("RecipientAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.RecipientBankName))
            throw new ArgumentException("RecipientBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.RecipientBankAddress))
            throw new ArgumentException("RecipientBankAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.RecipientAccountNumber))
            throw new ArgumentException("RecipientAccountNumber cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.RecipientSwiftCode))
            throw new ArgumentException("RecipientSwiftCode cannot be empty");

        if (newParam.Amount == null || newParam.Amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(newParam.JustificationForAdvance))
            throw new ArgumentException("JustificationForAdvance cannot be empty");

        AdvancePurpose = newParam.AdvancePurpose;
        RecipientName = newParam.RecipientName;
        RecipientAddress = newParam.RecipientAddress;
        RecipientBankName = newParam.RecipientBankName;
        RecipientBankAddress = newParam.RecipientBankAddress;
        RecipientAccountNumber = newParam.RecipientAccountNumber;
        RecipientSwiftCode = newParam.RecipientSwiftCode;
        Amount = newParam.Amount;
        ExpectedUsageDate = newParam.ExpectedUsageDate;
        JustificationForAdvance = newParam.JustificationForAdvance;
        RepaymentTerms = newParam.RepaymentTerms;
        SpecialInstructions = newParam.SpecialInstructions;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementA3(DisbursementA3LoadParam loadParam)
    {
        Id = loadParam.Id;
        DisbursementId = loadParam.DisbursementId;
        AdvancePurpose = loadParam.AdvancePurpose;
        RecipientName = loadParam.RecipientName;
        RecipientAddress = loadParam.RecipientAddress;
        RecipientBankName = loadParam.RecipientBankName;
        RecipientBankAddress = loadParam.RecipientBankAddress;
        RecipientAccountNumber = loadParam.RecipientAccountNumber;
        RecipientSwiftCode = loadParam.RecipientSwiftCode;
        Amount = loadParam.Amount;
        ExpectedUsageDate = loadParam.ExpectedUsageDate;
        JustificationForAdvance = loadParam.JustificationForAdvance;
        RepaymentTerms = loadParam.RepaymentTerms;
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
