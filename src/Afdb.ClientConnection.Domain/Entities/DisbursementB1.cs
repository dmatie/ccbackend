using Afdb.ClientConnection.Domain.Common;

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

    public DisbursementB1(
        Guid disbursementId,
        string guaranteeDetails,
        string confirmingBank,
        string issuingBankName,
        string issuingBankAdress,
        decimal guaranteeAmount,
        DateTime expiryDate,
        string beneficiaryName,
        string beneficiaryBPNumber,
        string beneficiaryAFDBContract,
        string beneficiaryBankAddress,
        string beneficiaryCity,
        Guid beneficiaryCountryId,
        string goodDescription,
        string beneficiaryLcContractRef,
        string executingAgencyName,
        string executingAgencyContactPerson,
        string executingAgencyAddress,
        string executingAgencyCity,
        Guid executingAgencyCountryId,
        string executingAgencyEmail,
        string executingAgencyPhone)
    {
        DisbursementId = disbursementId;
        GuaranteeDetails = guaranteeDetails;
        ConfirmingBank = confirmingBank;
        IssuingBankName = issuingBankName;
        IssuingBankAdress = issuingBankAdress;
        GuaranteeAmount = guaranteeAmount;
        ExpiryDate = expiryDate;
        BeneficiaryName = beneficiaryName;
        BeneficiaryBPNumber = beneficiaryBPNumber;
        BeneficiaryAFDBContract = beneficiaryAFDBContract;
        BeneficiaryBankAddress = beneficiaryBankAddress;
        BeneficiaryCity = beneficiaryCity;
        BeneficiaryCountryId = beneficiaryCountryId;
        GoodDescription = goodDescription;
        BeneficiaryLcContractRef = beneficiaryLcContractRef;
        ExecutingAgencyName = executingAgencyName;
        ExecutingAgencyContactPerson = executingAgencyContactPerson;
        ExecutingAgencyAddress = executingAgencyAddress;
        ExecutingAgencyCity = executingAgencyCity;
        ExecutingAgencyCountryId = executingAgencyCountryId;
        ExecutingAgencyEmail = executingAgencyEmail;
        ExecutingAgencyPhone = executingAgencyPhone;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementB1(
        Guid id,
        Guid disbursementId,
        string guaranteeDetails,
        string confirmingBank,
        string issuingBankName,
        string issuingBankAdress,
        decimal guaranteeAmount,
        DateTime expiryDate,
        string beneficiaryName,
        string beneficiaryBPNumber,
        string beneficiaryAFDBContract,
        string beneficiaryBankAddress,
        string beneficiaryCity,
        Guid beneficiaryCountryId,
        string goodDescription,
        string beneficiaryLcContractRef,
        string executingAgencyName,
        string executingAgencyContactPerson,
        string executingAgencyAddress,
        string executingAgencyCity,
        Guid executingAgencyCountryId,
        string executingAgencyEmail,
        string executingAgencyPhone,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt = null,
        string? updatedBy = null,
        Country? beneficiaryCountry = null,
        Country? executingAgencyCountry = null)
    {
        Id = id;
        DisbursementId = disbursementId;
        GuaranteeDetails = guaranteeDetails;
        ConfirmingBank = confirmingBank;
        IssuingBankName = issuingBankName;
        IssuingBankAdress = issuingBankAdress;
        GuaranteeAmount = guaranteeAmount;
        ExpiryDate = expiryDate;
        BeneficiaryName = beneficiaryName;
        BeneficiaryBPNumber = beneficiaryBPNumber;
        BeneficiaryAFDBContract = beneficiaryAFDBContract;
        BeneficiaryBankAddress = beneficiaryBankAddress;
        BeneficiaryCity = beneficiaryCity;
        BeneficiaryCountryId = beneficiaryCountryId;
        GoodDescription = goodDescription;
        BeneficiaryLcContractRef = beneficiaryLcContractRef;
        ExecutingAgencyName = executingAgencyName;
        ExecutingAgencyContactPerson = executingAgencyContactPerson;
        ExecutingAgencyAddress = executingAgencyAddress;
        ExecutingAgencyCity = executingAgencyCity;
        ExecutingAgencyCountryId = executingAgencyCountryId;
        ExecutingAgencyEmail = executingAgencyEmail;
        ExecutingAgencyPhone = executingAgencyPhone;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        BeneficiaryCountry = beneficiaryCountry;
        ExecutingAgencyCountry = executingAgencyCountry;
    }
}
