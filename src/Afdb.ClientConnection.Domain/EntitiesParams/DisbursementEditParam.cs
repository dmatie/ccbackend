using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementEditParam
{
    public required string SapCodeProject { get; init; }
    public required string LoanGrantNumber { get; init; }
    public required Guid DisbursementTypeId { get; init; }
    public required Guid CreatedByUserId { get; init; }
    public required Guid CurrencyId { get; init; }
}
