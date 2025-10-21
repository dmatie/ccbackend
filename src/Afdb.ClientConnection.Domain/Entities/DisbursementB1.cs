using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementB1 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    
    public string GuaranteeDetails { get; private set; }
    public string ConfirmingBank { get; private set; }
    
    public string IssuingBankName { get; private set; }
    public string IssuingBankAdress { get; private set; }
    public decimal GuaranteeAmount { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    
    public string BeneficiaryName { get; private set; }
    public string BeneficiaryBPNumber { get; private set; }
    public string BeneficiaryAFDBContract { get; private set; }
    public string BeneficiaryBankAddress { get; private set; }
    public string BeneficiaryCity { get; private set; }
    public Guid BeneficiaryCountryId { get; private set; }
    public string GoodDescription { get; private set; }
    public string BeneficiaryLcContractRef { get; private set; }
    
    public string ExecutingAgencyName { get; private set; }
    public string ExecutingAgencyContactPerson { get; private set; }
    public string ExecutingAgencyAddress { get; private set; }
    public string ExecutingAgencyCity { get; private set; }
    public Guid ExecutingAgencyCountryId { get; private set; }
    public string ExecutingAgencyEmail { get; private set; }
    public string ExecutingAgencyPhone { get; private set; }

    public Country? BeneficiaryCountry { get; private set; }
    public Country? ExecutingAgencyCountry { get; private set; }

    private DisbursementB1() { }

    public DisbursementB1(DisbursementB1NewParam param)
    {
        DisbursementId = param.DisbursementId;
        GuaranteeDetails = param.GuaranteeDetails;
        ConfirmingBank = param.ConfirmingBank;
        IssuingBankName = param.IssuingBankName;
        IssuingBankAdress = param.IssuingBankAdress;
        GuaranteeAmount = param.GuaranteeAmount;
        ExpiryDate = param.ExpiryDate;
        BeneficiaryName = param.BeneficiaryName;
        BeneficiaryBPNumber = param.BeneficiaryBPNumber;
        BeneficiaryAFDBContract = param.BeneficiaryAFDBContract;
        BeneficiaryBankAddress = param.BeneficiaryBankAddress;
        BeneficiaryCity = param.BeneficiaryCity;
        BeneficiaryCountryId = param.BeneficiaryCountryId;
        GoodDescription = param.GoodDescription;
        BeneficiaryLcContractRef = param.BeneficiaryLcContractRef;
        ExecutingAgencyName = param.ExecutingAgencyName;
        ExecutingAgencyContactPerson = param.ExecutingAgencyContactPerson;
        ExecutingAgencyAddress = param.ExecutingAgencyAddress;
        ExecutingAgencyCity = param.ExecutingAgencyCity;
        ExecutingAgencyCountryId = param.ExecutingAgencyCountryId;
        ExecutingAgencyEmail = param.ExecutingAgencyEmail;
        ExecutingAgencyPhone = param.ExecutingAgencyPhone;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementB1(DisbursementB1LoadParam param)
    {
        Id = param.Id;
        DisbursementId = param.DisbursementId;
        GuaranteeDetails = param.GuaranteeDetails;
        ConfirmingBank = param.ConfirmingBank;
        IssuingBankName = param.IssuingBankName;
        IssuingBankAdress = param.IssuingBankAdress;
        GuaranteeAmount = param.GuaranteeAmount;
        ExpiryDate = param.ExpiryDate;
        BeneficiaryName = param.BeneficiaryName;
        BeneficiaryBPNumber = param.BeneficiaryBPNumber;
        BeneficiaryAFDBContract = param.BeneficiaryAFDBContract;
        BeneficiaryBankAddress = param.BeneficiaryBankAddress;
        BeneficiaryCity = param.BeneficiaryCity;
        BeneficiaryCountryId = param.BeneficiaryCountryId;
        GoodDescription = param.GoodDescription;
        BeneficiaryLcContractRef = param.BeneficiaryLcContractRef;
        ExecutingAgencyName = param.ExecutingAgencyName;
        ExecutingAgencyContactPerson = param.ExecutingAgencyContactPerson;
        ExecutingAgencyAddress = param.ExecutingAgencyAddress;
        ExecutingAgencyCity = param.ExecutingAgencyCity;
        ExecutingAgencyCountryId = param.ExecutingAgencyCountryId;
        ExecutingAgencyEmail = param.ExecutingAgencyEmail;
        ExecutingAgencyPhone = param.ExecutingAgencyPhone;
        CreatedAt = param.CreatedAt;
        CreatedBy = param.CreatedBy;
        UpdatedAt = param.UpdatedAt;
        UpdatedBy = param.UpdatedBy;
        BeneficiaryCountry = param.BeneficiaryCountry;
        ExecutingAgencyCountry = param.ExecutingAgencyCountry;
    }
}
