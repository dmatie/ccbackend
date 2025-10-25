using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

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
    public string CorrespondantAccountNumber { get; private set; }
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

    public DisbursementA1(DisbursementA1NewParam param)
    {
        PaymentPurpose = param.PaymentPurpose;
        BeneficiaryBpNumber = param.BeneficiaryBpNumber;
        BeneficiaryName = param.BeneficiaryName;
        BeneficiaryContactPerson = param.BeneficiaryContactPerson;
        BeneficiaryAddress = param.BeneficiaryAddress;
        BeneficiaryCountryId = param.BeneficiaryCountryId;
        BeneficiaryEmail = param.BeneficiaryEmail;
        CorrespondentBankName = param.CorrespondentBankName;
        CorrespondentBankAddress = param.CorrespondentBankAddress;
        CorrespondentBankCountryId = param.CorrespondentBankCountryId;
        CorrespondantAccountNumber = param.CorrespondantAccountNumber;
        CorrespondentBankSwiftCode = param.CorrespondentBankSwiftCode;
        Amount = param.Amount;
        SignatoryName = param.SignatoryName;
        SignatoryContactPerson = param.SignatoryContactPerson;
        SignatoryAddress = param.SignatoryAddress;
        SignatoryCountryId = param.SignatoryCountryId;
        SignatoryEmail = param.SignatoryEmail;
        SignatoryPhone = param.SignatoryPhone;
        SignatoryTitle = param.SignatoryTitle;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementA1(DisbursementA1LoadParam param)
    {
        Id = param.Id;
        DisbursementId = param.DisbursementId;
        PaymentPurpose = param.PaymentPurpose;
        BeneficiaryBpNumber = param.BeneficiaryBpNumber;
        BeneficiaryName = param.BeneficiaryName;
        BeneficiaryContactPerson = param.BeneficiaryContactPerson;
        BeneficiaryAddress = param.BeneficiaryAddress;
        BeneficiaryCountryId = param.BeneficiaryCountryId;
        BeneficiaryEmail = param.BeneficiaryEmail;
        CorrespondentBankName = param.CorrespondentBankName;
        CorrespondentBankAddress = param.CorrespondentBankAddress;
        CorrespondentBankCountryId = param.CorrespondentBankCountryId;
        CorrespondantAccountNumber = param.CorrespondantAccountNumber;
        CorrespondentBankSwiftCode = param.CorrespondentBankSwiftCode;
        Amount = param.Amount;
        SignatoryName = param.SignatoryName;
        SignatoryContactPerson = param.SignatoryContactPerson;
        SignatoryAddress = param.SignatoryAddress;
        SignatoryCountryId = param.SignatoryCountryId;
        SignatoryEmail = param.SignatoryEmail;
        SignatoryPhone = param.SignatoryPhone;
        SignatoryTitle = param.SignatoryTitle;
        CreatedAt = param.CreatedAt;
        CreatedBy = param.CreatedBy;
        UpdatedAt = param.UpdatedAt;
        UpdatedBy = param.UpdatedBy;
        BeneficiaryCountry = param.BeneficiaryCountry;
        CorrespondentBankCountry = param.CorrespondentBankCountry;
        SignatoryCountry = param.SignatoryCountry;
    }
}
