namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA1NewParam(
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
    string CorrespondantAccountNumber,
    string CorrespondentBankSwiftCode,
    decimal Amount,
    string SignatoryName,
    string SignatoryContactPerson,
    string SignatoryAddress,
    Guid SignatoryCountryId,
    string SignatoryEmail,
    string SignatoryPhone,
    string SignatoryTitle
);
