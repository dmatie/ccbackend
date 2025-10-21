namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementA3NewParam(
    Guid DisbursementId,
    string PeriodForUtilization,
    int ItemNumber,
    string GoodDescription,
    Guid GoodOriginCountryId,
    int GoodQuantity,
    decimal AnnualBudget,
    decimal BankShare,
    decimal AdvanceRequested,
    DateTime DateOfApproval
);
