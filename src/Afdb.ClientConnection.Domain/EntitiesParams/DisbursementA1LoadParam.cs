using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;
public sealed record DisbursementA1LoadParam : CommonLoadParam
{
    public Guid DisbursementId { get; init; }
    public string PaymentPurpose { get; init; } = default!;
    public string BeneficiaryBpNumber { get; init; } = default!;
    public string BeneficiaryName { get; init; } = default!;
    public string BeneficiaryContactPerson { get; init; } = default!;
    public string BeneficiaryAddress { get; init; } = default!;
    public Guid BeneficiaryCountryId { get; init; }
    public string BeneficiaryEmail { get; init; } = default!;
    public string CorrespondentBankName { get; init; } = default!;
    public string CorrespondentBankAddress { get; init; } = default!;
    public Guid CorrespondentBankCountryId { get; init; }
    public string CorrespondantAccountNumber { get; init; } = default!;
    public string CorrespondentBankSwiftCode { get; init; } = default!;
    public decimal Amount { get; init; }
    public string SignatoryName { get; init; } = default!;
    public string SignatoryContactPerson { get; init; } = default!;
    public string SignatoryAddress { get; init; } = default!;
    public Guid SignatoryCountryId { get; init; }
    public string SignatoryEmail { get; init; } = default!;
    public string SignatoryPhone { get; init; } = default!;
    public string SignatoryTitle { get; init; } = default!;

    public Country? BeneficiaryCountry { get; init; } = default!;
    public Country? CorrespondentBankCountry { get; init; } = default;
    public Country? SignatoryCountry { get; init; } = default;
}
