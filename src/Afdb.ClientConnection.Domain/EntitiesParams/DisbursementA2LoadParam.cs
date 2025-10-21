using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA2LoadParam(
    Guid Id,
    Guid DisbursementId,
    string ReimbursementPurpose,
    string Contractor,
    string GoodDescription,
    Guid GoodOriginCountryId,
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
    string PaymentEvidenceOfPayment,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt = null,
    string? UpdatedBy = null,
    Country? GoodOriginCountry = null
);
