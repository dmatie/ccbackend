using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementA2 : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public string ReimbursementPurpose { get; private set; }
    public string Contractor { get; private set; }
    
    public string GoodDescription { get; private set; }
    public Guid GoodOrginCountryId { get; private set; }
    
    public string ContractBorrowerReference { get; private set; }
    public string ContractAfDBReference { get; private set; }
    public string ContractValue { get; private set; }
    public string ContractBankShare { get; private set; }
    public decimal ContractAmountPreviouslyPaid { get; private set; }
    
    public string InvoiceRef { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public decimal InvoiceAmount { get; private set; }
    
    public DateTime PaymentDateOfPayment { get; private set; }
    public decimal PaymentAmountWithdrawn { get; private set; }
    public string PaymentEvidenceOfPayment { get; private set; }

    public Country? GoodOrginCountry { get; private set; }

    private DisbursementA2() { }

    public DisbursementA2(DisbursementA2NewParam param)
    {
        DisbursementId = param.DisbursementId;
        ReimbursementPurpose = param.ReimbursementPurpose;
        Contractor = param.Contractor;
        GoodDescription = param.GoodDescription;
        GoodOrginCountryId = param.GoodOrginCountryId;
        ContractBorrowerReference = param.ContractBorrowerReference;
        ContractAfDBReference = param.ContractAfDBReference;
        ContractValue = param.ContractValue;
        ContractBankShare = param.ContractBankShare;
        ContractAmountPreviouslyPaid = param.ContractAmountPreviouslyPaid;
        InvoiceRef = param.InvoiceRef;
        InvoiceDate = param.InvoiceDate;
        InvoiceAmount = param.InvoiceAmount;
        PaymentDateOfPayment = param.PaymentDateOfPayment;
        PaymentAmountWithdrawn = param.PaymentAmountWithdrawn;
        PaymentEvidenceOfPayment = param.PaymentEvidenceOfPayment;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementA2(DisbursementA2LoadParam param)
    {
        Id = param.Id;
        DisbursementId = param.DisbursementId;
        ReimbursementPurpose = param.ReimbursementPurpose;
        Contractor = param.Contractor;
        GoodDescription = param.GoodDescription;
        GoodOrginCountryId = param.GoodOrginCountryId;
        ContractBorrowerReference = param.ContractBorrowerReference;
        ContractAfDBReference = param.ContractAfDBReference;
        ContractValue = param.ContractValue;
        ContractBankShare = param.ContractBankShare;
        ContractAmountPreviouslyPaid = param.ContractAmountPreviouslyPaid;
        InvoiceRef = param.InvoiceRef;
        InvoiceDate = param.InvoiceDate;
        InvoiceAmount = param.InvoiceAmount;
        PaymentDateOfPayment = param.PaymentDateOfPayment;
        PaymentAmountWithdrawn = param.PaymentAmountWithdrawn;
        PaymentEvidenceOfPayment = param.PaymentEvidenceOfPayment;
        CreatedAt = param.CreatedAt;
        CreatedBy = param.CreatedBy;
        UpdatedAt = param.UpdatedAt;
        UpdatedBy = param.UpdatedBy;
        GoodOrginCountry = param.GoodOrginCountry;
    }
}
