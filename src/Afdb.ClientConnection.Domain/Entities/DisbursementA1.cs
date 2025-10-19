using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementA1 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string PaymentPurpose { get; private set; }
    public string BeneficiaryName { get; private set; }
    public string BeneficiaryAddress { get; private set; }
    public string BeneficiaryBankName { get; private set; }
    public string BeneficiaryBankAddress { get; private set; }
    public string BeneficiaryAccountNumber { get; private set; }
    public string BeneficiarySwiftCode { get; private set; }
    public Money Amount { get; private set; }
    public string? IntermediaryBankName { get; private set; }
    public string? IntermediaryBankSwiftCode { get; private set; }
    public string? SpecialInstructions { get; private set; }

    public Disbursement? Disbursement { get; private set; }

    private DisbursementA1() { }

    public DisbursementA1(DisbursementA1NewParam newParam)
    {
        if (string.IsNullOrWhiteSpace(newParam.PaymentPurpose))
            throw new ArgumentException("PaymentPurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryName))
            throw new ArgumentException("BeneficiaryName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryAddress))
            throw new ArgumentException("BeneficiaryAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryBankName))
            throw new ArgumentException("BeneficiaryBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryBankAddress))
            throw new ArgumentException("BeneficiaryBankAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryAccountNumber))
            throw new ArgumentException("BeneficiaryAccountNumber cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiarySwiftCode))
            throw new ArgumentException("BeneficiarySwiftCode cannot be empty");

        if (newParam.Amount == null || newParam.Amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        PaymentPurpose = newParam.PaymentPurpose;
        BeneficiaryName = newParam.BeneficiaryName;
        BeneficiaryAddress = newParam.BeneficiaryAddress;
        BeneficiaryBankName = newParam.BeneficiaryBankName;
        BeneficiaryBankAddress = newParam.BeneficiaryBankAddress;
        BeneficiaryAccountNumber = newParam.BeneficiaryAccountNumber;
        BeneficiarySwiftCode = newParam.BeneficiarySwiftCode;
        Amount = newParam.Amount;
        IntermediaryBankName = newParam.IntermediaryBankName;
        IntermediaryBankSwiftCode = newParam.IntermediaryBankSwiftCode;
        SpecialInstructions = newParam.SpecialInstructions;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementA1(DisbursementA1LoadParam loadParam)
    {
        Id = loadParam.Id;
        DisbursementId = loadParam.DisbursementId;
        PaymentPurpose = loadParam.PaymentPurpose;
        BeneficiaryName = loadParam.BeneficiaryName;
        BeneficiaryAddress = loadParam.BeneficiaryAddress;
        BeneficiaryBankName = loadParam.BeneficiaryBankName;
        BeneficiaryBankAddress = loadParam.BeneficiaryBankAddress;
        BeneficiaryAccountNumber = loadParam.BeneficiaryAccountNumber;
        BeneficiarySwiftCode = loadParam.BeneficiarySwiftCode;
        Amount = loadParam.Amount;
        IntermediaryBankName = loadParam.IntermediaryBankName;
        IntermediaryBankSwiftCode = loadParam.IntermediaryBankSwiftCode;
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
