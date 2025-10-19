using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementB1 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string GuaranteePurpose { get; private set; }
    public string BeneficiaryName { get; private set; }
    public string BeneficiaryAddress { get; private set; }
    public string BeneficiaryBankName { get; private set; }
    public string BeneficiaryBankAddress { get; private set; }
    public Money GuaranteeAmount { get; private set; }
    public DateTime ValidityStartDate { get; private set; }
    public DateTime ValidityEndDate { get; private set; }
    public string GuaranteeTermsAndConditions { get; private set; }
    public string? SpecialInstructions { get; private set; }

    public Disbursement? Disbursement { get; private set; }

    private DisbursementB1() { }

    public DisbursementB1(
        string guaranteePurpose,
        string beneficiaryName,
        string beneficiaryAddress,
        string beneficiaryBankName,
        string beneficiaryBankAddress,
        Money guaranteeAmount,
        DateTime validityStartDate,
        DateTime validityEndDate,
        string guaranteeTermsAndConditions,
        string? specialInstructions,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(guaranteePurpose))
            throw new ArgumentException("GuaranteePurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryName))
            throw new ArgumentException("BeneficiaryName cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryAddress))
            throw new ArgumentException("BeneficiaryAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryBankName))
            throw new ArgumentException("BeneficiaryBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(beneficiaryBankAddress))
            throw new ArgumentException("BeneficiaryBankAddress cannot be empty");

        if (guaranteeAmount == null || guaranteeAmount.Amount <= 0)
            throw new ArgumentException("GuaranteeAmount must be greater than zero");

        if (validityEndDate <= validityStartDate)
            throw new ArgumentException("ValidityEndDate must be after ValidityStartDate");

        if (string.IsNullOrWhiteSpace(guaranteeTermsAndConditions))
            throw new ArgumentException("GuaranteeTermsAndConditions cannot be empty");

        GuaranteePurpose = guaranteePurpose;
        BeneficiaryName = beneficiaryName;
        BeneficiaryAddress = beneficiaryAddress;
        BeneficiaryBankName = beneficiaryBankName;
        BeneficiaryBankAddress = beneficiaryBankAddress;
        GuaranteeAmount = guaranteeAmount;
        ValidityStartDate = validityStartDate;
        ValidityEndDate = validityEndDate;
        GuaranteeTermsAndConditions = guaranteeTermsAndConditions;
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
