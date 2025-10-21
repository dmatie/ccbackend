using Afdb.ClientConnection.Domain.Common;

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

    public DisbursementA2(
        Guid disbursementId,
        string reimbursementPurpose,
        string contractor,
        string goodDescription,
        Guid goodOrginCountryId,
        string contractBorrowerReference,
        string contractAfDBReference,
        string contractValue,
        string contractBankShare,
        decimal contractAmountPreviouslyPaid,
        string invoiceRef,
        DateTime invoiceDate,
        decimal invoiceAmount,
        DateTime paymentDateOfPayment,
        decimal paymentAmountWithdrawn,
        string paymentEvidenceOfPayment)
    {
        DisbursementId = disbursementId;
        ReimbursementPurpose = reimbursementPurpose;
        Contractor = contractor;
        GoodDescription = goodDescription;
        GoodOrginCountryId = goodOrginCountryId;
        ContractBorrowerReference = contractBorrowerReference;
        ContractAfDBReference = contractAfDBReference;
        ContractValue = contractValue;
        ContractBankShare = contractBankShare;
        ContractAmountPreviouslyPaid = contractAmountPreviouslyPaid;
        InvoiceRef = invoiceRef;
        InvoiceDate = invoiceDate;
        InvoiceAmount = invoiceAmount;
        PaymentDateOfPayment = paymentDateOfPayment;
        PaymentAmountWithdrawn = paymentAmountWithdrawn;
        PaymentEvidenceOfPayment = paymentEvidenceOfPayment;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementA2(
        Guid id,
        Guid disbursementId,
        string reimbursementPurpose,
        string contractor,
        string goodDescription,
        Guid goodOrginCountryId,
        string contractBorrowerReference,
        string contractAfDBReference,
        string contractValue,
        string contractBankShare,
        decimal contractAmountPreviouslyPaid,
        string invoiceRef,
        DateTime invoiceDate,
        decimal invoiceAmount,
        DateTime paymentDateOfPayment,
        decimal paymentAmountWithdrawn,
        string paymentEvidenceOfPayment,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt = null,
        string? updatedBy = null,
        Country? goodOrginCountry = null)
    {
        Id = id;
        DisbursementId = disbursementId;
        ReimbursementPurpose = reimbursementPurpose;
        Contractor = contractor;
        GoodDescription = goodDescription;
        GoodOrginCountryId = goodOrginCountryId;
        ContractBorrowerReference = contractBorrowerReference;
        ContractAfDBReference = contractAfDBReference;
        ContractValue = contractValue;
        ContractBankShare = contractBankShare;
        ContractAmountPreviouslyPaid = contractAmountPreviouslyPaid;
        InvoiceRef = invoiceRef;
        InvoiceDate = invoiceDate;
        InvoiceAmount = invoiceAmount;
        PaymentDateOfPayment = paymentDateOfPayment;
        PaymentAmountWithdrawn = paymentAmountWithdrawn;
        PaymentEvidenceOfPayment = paymentEvidenceOfPayment;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        GoodOrginCountry = goodOrginCountry;
    }
}
