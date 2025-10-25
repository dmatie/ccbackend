using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA2LoadParam : CommonLoadParam
{
    public Guid DisbursementId { get; init; }
    public string ReimbursementPurpose { get; init; } = default!;
    public string Contractor { get; init; } = default!;
    public string GoodDescription { get; init; } = default!;
    public Guid GoodOrginCountryId { get; init; }
    public string ContractBorrowerReference { get; init; } = default!;
    public string ContractAfDBReference { get; init; } = default!;
    public string ContractValue { get; init; } = default!;
    public string ContractBankShare { get; init; } = default!;
    public decimal ContractAmountPreviouslyPaid { get; init; }
    public string InvoiceRef { get; init; } = default!;
    public DateTime InvoiceDate { get; init; }
    public decimal InvoiceAmount { get; init; }
    public DateTime PaymentDateOfPayment { get; init; }
    public decimal PaymentAmountWithdrawn { get; init; }
    public string PaymentEvidenceOfPayment { get; init; } = default!;
    public Country? GoodOrginCountry { get; init; } = default!;
}
