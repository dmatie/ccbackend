using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementA1 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string PaymentPurpose { get; private set; }
    
    public string BeneficiaryBpNumber { get; private set; }
    public string BeneficiaryName { get; private set; }
    public string BeneficiaryContactPerson { get; private set; }
    public string BeneficiaryAddress { get; private set; }
    public Guid BeneficiaryCountryId { get; private set; }
    public string BeneficiaryEmail { get; private set; }
    
    public string CorrespondentBankName { get; private set; }
    public string CorrespondentBankAddress { get; private set; }
    public Guid CorrespondentBankCountryId { get; private set; }
    public string CorrespondentAccountNumber { get; private set; }
    public string CorrespondentBankSwiftCode { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public string SignatoryName { get; private set; }
    public string SignatoryContactPerson { get; private set; }
    public string SignatoryAddress { get; private set; }
    public Guid SignatoryCountryId { get; private set; }
    public string SignatoryEmail { get; private set; }
    public string SignatoryPhone { get; private set; }
    public string SignatoryTitle { get; private set; }

    public Country? BeneficiaryCountry { get; private set; }
    public Country? CorrespondentBankCountry { get; private set; }
    public Country? SignatoryCountry { get; private set; }

    private DisbursementA1() { }

    public DisbursementA1(
        Guid disbursementId,
        string paymentPurpose,
        string beneficiaryBpNumber,
        string beneficiaryName,
        string beneficiaryContactPerson,
        string beneficiaryAddress,
        Guid beneficiaryCountryId,
        string beneficiaryEmail,
        string correspondentBankName,
        string correspondentBankAddress,
        Guid correspondentBankCountryId,
        string correspondentAccountNumber,
        string correspondentBankSwiftCode,
        decimal amount,
        string signatoryName,
        string signatoryContactPerson,
        string signatoryAddress,
        Guid signatoryCountryId,
        string signatoryEmail,
        string signatoryPhone,
        string signatoryTitle)
    {
        DisbursementId = disbursementId;
        PaymentPurpose = paymentPurpose;
        BeneficiaryBpNumber = beneficiaryBpNumber;
        BeneficiaryName = beneficiaryName;
        BeneficiaryContactPerson = beneficiaryContactPerson;
        BeneficiaryAddress = beneficiaryAddress;
        BeneficiaryCountryId = beneficiaryCountryId;
        BeneficiaryEmail = beneficiaryEmail;
        CorrespondentBankName = correspondentBankName;
        CorrespondentBankAddress = correspondentBankAddress;
        CorrespondentBankCountryId = correspondentBankCountryId;
        CorrespondentAccountNumber = correspondentAccountNumber;
        CorrespondentBankSwiftCode = correspondentBankSwiftCode;
        Amount = amount;
        SignatoryName = signatoryName;
        SignatoryContactPerson = signatoryContactPerson;
        SignatoryAddress = signatoryAddress;
        SignatoryCountryId = signatoryCountryId;
        SignatoryEmail = signatoryEmail;
        SignatoryPhone = signatoryPhone;
        SignatoryTitle = signatoryTitle;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementA1(
        Guid id,
        Guid disbursementId,
        string paymentPurpose,
        string beneficiaryBpNumber,
        string beneficiaryName,
        string beneficiaryContactPerson,
        string beneficiaryAddress,
        Guid beneficiaryCountryId,
        string beneficiaryEmail,
        string correspondentBankName,
        string correspondentBankAddress,
        Guid correspondentBankCountryId,
        string correspondentAccountNumber,
        string correspondentBankSwiftCode,
        decimal amount,
        string signatoryName,
        string signatoryContactPerson,
        string signatoryAddress,
        Guid signatoryCountryId,
        string signatoryEmail,
        string signatoryPhone,
        string signatoryTitle,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt = null,
        string? updatedBy = null,
        Country? beneficiaryCountry = null,
        Country? correspondentBankCountry = null,
        Country? signatoryCountry = null)
    {
        Id = id;
        DisbursementId = disbursementId;
        PaymentPurpose = paymentPurpose;
        BeneficiaryBpNumber = beneficiaryBpNumber;
        BeneficiaryName = beneficiaryName;
        BeneficiaryContactPerson = beneficiaryContactPerson;
        BeneficiaryAddress = beneficiaryAddress;
        BeneficiaryCountryId = beneficiaryCountryId;
        BeneficiaryEmail = beneficiaryEmail;
        CorrespondentBankName = correspondentBankName;
        CorrespondentBankAddress = correspondentBankAddress;
        CorrespondentBankCountryId = correspondentBankCountryId;
        CorrespondentAccountNumber = correspondentAccountNumber;
        CorrespondentBankSwiftCode = correspondentBankSwiftCode;
        Amount = amount;
        SignatoryName = signatoryName;
        SignatoryContactPerson = signatoryContactPerson;
        SignatoryAddress = signatoryAddress;
        SignatoryCountryId = signatoryCountryId;
        SignatoryEmail = signatoryEmail;
        SignatoryPhone = signatoryPhone;
        SignatoryTitle = signatoryTitle;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        BeneficiaryCountry = beneficiaryCountry;
        CorrespondentBankCountry = correspondentBankCountry;
        SignatoryCountry = signatoryCountry;
    }
}
