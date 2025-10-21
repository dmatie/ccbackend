using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementB1LoadParam(
    Guid Id,
    Guid DisbursementId,
    string GuaranteeDetails,
    string ConfirmingBank,
    string IssuingBankName,
    string IssuingBankAddress,
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
    string ExecutingAgencyPhone,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt = null,
    string? UpdatedBy = null,
    Country? BeneficiaryCountry = null,
    Country? ExecutingAgencyCountry = null
);
