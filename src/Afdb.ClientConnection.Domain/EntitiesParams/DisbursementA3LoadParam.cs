using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA3LoadParam(
    Guid Id,
    Guid DisbursementId,
    string PeriodForUtilization,
    int ItemNumber,
    string GoodDescription,
    Guid GoodOrginCountryId,
    int GoodQuantity,
    decimal AnnualBudget,
    decimal BankShare,
    decimal AdvanceRequested,
    DateTime DateOfApproval,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt = null,
    string? UpdatedBy = null,
    Country? GoodOrginCountry = null
);
