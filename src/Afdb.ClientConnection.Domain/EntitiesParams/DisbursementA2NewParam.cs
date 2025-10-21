namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA2NewParam(
    Guid DisbursementId,
    string ReimbursementPurpose,
    string Contractor,
    string GoodDescription,
    Guid GoodOrginCountryId,
    string ContractBorrowerReference,
    string ContractAfDBReference,
    string ContractValue,
    string ContractBankShare,
    decimal ContractAmountPreviouslyPaid,
    string InvoiceRef,
    DateTime InvoiceDate,
    decimal InvoiceAmount,
    DateTime PaymentDateOfPayment,
    decimal PaymentAmountWithdrawn,
    string PaymentEvidenceOfPayment
);
