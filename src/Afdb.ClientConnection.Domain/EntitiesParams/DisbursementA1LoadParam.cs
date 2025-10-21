using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA1LoadParam(
    Guid Id,
    Guid DisbursementId,
    string PaymentPurpose,
    string BeneficiaryBpNumber,
    string BeneficiaryName,
    string BeneficiaryContactPerson,
    string BeneficiaryAddress,
    Guid BeneficiaryCountryId,
    string BeneficiaryEmail,
    string CorrespondentBankName,
    string CorrespondentBankAddress,
    Guid CorrespondentBankCountryId,
    string CorrespondentAccountNumber,
    string CorrespondentBankSwiftCode,
    decimal Amount,
    string SignatoryName,
    string SignatoryContactPerson,
    string SignatoryAddress,
    Guid SignatoryCountryId,
    string SignatoryEmail,
    string SignatoryPhone,
    string SignatoryTitle,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt = null,
    string? UpdatedBy = null,
    Country? BeneficiaryCountry = null,
    Country? CorrespondentBankCountry = null,
    Country? SignatoryCountry = null
);
