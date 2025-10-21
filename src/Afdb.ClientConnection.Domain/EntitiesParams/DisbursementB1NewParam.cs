namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementB1NewParam(
    Guid DisbursementId,
    string GuaranteeDetails,
    string ConfirmingBank,
    string IssuingBankName,
    string IssuingBankAdress,
    decimal GuaranteeAmount,
    DateTime ExpiryDate,
    string BeneficiaryName,
    string BeneficiaryBPNumber,
    string BeneficiaryAFDBContract,
    string BeneficiaryBankAddress,
    string BeneficiaryCity,
    Guid BeneficiaryCountryId,
    string GoodDescription,
    string BeneficiaryLcContractRef,
    string ExecutingAgencyName,
    string ExecutingAgencyContactPerson,
    string ExecutingAgencyAddress,
    string ExecutingAgencyCity,
    Guid ExecutingAgencyCountryId,
    string ExecutingAgencyEmail,
    string ExecutingAgencyPhone
);
