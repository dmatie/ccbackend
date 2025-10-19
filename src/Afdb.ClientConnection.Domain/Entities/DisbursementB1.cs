using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
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

    public DisbursementB1(DisbursementB1NewParam newParam)
    {
        if (string.IsNullOrWhiteSpace(newParam.GuaranteePurpose))
            throw new ArgumentException("GuaranteePurpose cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryName))
            throw new ArgumentException("BeneficiaryName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryAddress))
            throw new ArgumentException("BeneficiaryAddress cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryBankName))
            throw new ArgumentException("BeneficiaryBankName cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.BeneficiaryBankAddress))
            throw new ArgumentException("BeneficiaryBankAddress cannot be empty");

        if (newParam.GuaranteeAmount == null || newParam.GuaranteeAmount.Amount <= 0)
            throw new ArgumentException("GuaranteeAmount must be greater than zero");

        if (newParam.ValidityEndDate <= newParam.ValidityStartDate)
            throw new ArgumentException("ValidityEndDate must be after ValidityStartDate");

        if (string.IsNullOrWhiteSpace(newParam.GuaranteeTermsAndConditions))
            throw new ArgumentException("GuaranteeTermsAndConditions cannot be empty");

        GuaranteePurpose = newParam.GuaranteePurpose;
        BeneficiaryName = newParam.BeneficiaryName;
        BeneficiaryAddress = newParam.BeneficiaryAddress;
        BeneficiaryBankName = newParam.BeneficiaryBankName;
        BeneficiaryBankAddress = newParam.BeneficiaryBankAddress;
        GuaranteeAmount = newParam.GuaranteeAmount;
        ValidityStartDate = newParam.ValidityStartDate;
        ValidityEndDate = newParam.ValidityEndDate;
        GuaranteeTermsAndConditions = newParam.GuaranteeTermsAndConditions;
        SpecialInstructions = newParam.SpecialInstructions;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementB1(DisbursementB1LoadParam loadParam)
    {
        Id = loadParam.Id;
        DisbursementId = loadParam.DisbursementId;
        GuaranteePurpose = loadParam.GuaranteePurpose;
        BeneficiaryName = loadParam.BeneficiaryName;
        BeneficiaryAddress = loadParam.BeneficiaryAddress;
        BeneficiaryBankName = loadParam.BeneficiaryBankName;
        BeneficiaryBankAddress = loadParam.BeneficiaryBankAddress;
        GuaranteeAmount = loadParam.GuaranteeAmount;
        ValidityStartDate = loadParam.ValidityStartDate;
        ValidityEndDate = loadParam.ValidityEndDate;
        GuaranteeTermsAndConditions = loadParam.GuaranteeTermsAndConditions;
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
