using Afdb.ClientConnection.Domain.Common;
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

    public DisbursementA1(
        string paymentPurpose,
        string beneficiaryName,
        string beneficiaryAddress,
        string beneficiaryBankName,
        string beneficiaryBankAddress,
        string beneficiaryAccountNumber,
        string beneficiarySwiftCode,
        Money amount,
        string? intermediaryBankName,
        string? intermediaryBankSwiftCode,
        string? specialInstructions,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(paymentPurpose))
            throw new ArgumentException("PaymentPurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryName))
            throw new ArgumentException("BeneficiaryName cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryAddress))
            throw new ArgumentException("BeneficiaryAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryBankName))
            throw new ArgumentException("BeneficiaryBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryBankAddress))
            throw new ArgumentException("BeneficiaryBankAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryAccountNumber))
            throw new ArgumentException("BeneficiaryAccountNumber cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiarySwiftCode))
            throw new ArgumentException("BeneficiarySwiftCode cannot be empty");

        if (amount == null || amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        PaymentPurpose = paymentPurpose;
        BeneficiaryName = beneficiaryName;
        BeneficiaryAddress = beneficiaryAddress;
        BeneficiaryBankName = beneficiaryBankName;
        BeneficiaryBankAddress = beneficiaryBankAddress;
        BeneficiaryAccountNumber = beneficiaryAccountNumber;
        BeneficiarySwiftCode = beneficiarySwiftCode;
        Amount = amount;
        IntermediaryBankName = intermediaryBankName;
        IntermediaryBankSwiftCode = intermediaryBankSwiftCode;
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
