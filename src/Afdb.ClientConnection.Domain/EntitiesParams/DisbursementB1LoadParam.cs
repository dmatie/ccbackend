using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementB1LoadParam : CommonLoadParam
{
    public Guid DisbursementId { get; init; }
    public string GuaranteeDetails { get; init; } = default!;
    public string ConfirmingBank { get; init; } = default!;
    public string IssuingBankName { get; init; } = default!;
    public string IssuingBankAdress { get; init; } = default!;
    public decimal GuaranteeAmount { get; init; }
    public DateTime ExpiryDate { get; init; }
    public string BeneficiaryName { get; init; } = default!;
    public string BeneficiaryBPNumber { get; init; } = default!;
    public string BeneficiaryAFDBContract { get; init; } = default!;
    public string BeneficiaryBankAddress { get; init; } = default!;
    public string BeneficiaryCity { get; init; } = default!;
    public Guid BeneficiaryCountryId { get; init; }
    public string GoodDescription { get; init; } = default!;
    public string BeneficiaryLcContractRef { get; init; } = default!;
    public string ExecutingAgencyName { get; init; } = default!;
    public string ExecutingAgencyContactPerson { get; init; } = default!;
    public string ExecutingAgencyAddress { get; init; } = default!;
    public string ExecutingAgencyCity { get; init; } = default!;
    public Guid ExecutingAgencyCountryId { get; init; }
    public string ExecutingAgencyEmail { get; init; } = default!;
    public string ExecutingAgencyPhone { get; init; } = default!;

    public Country? BeneficiaryCountry { get; init; } = default!;
    public Country? ExecutingAgencyCountry { get; init; } = default!;
}
