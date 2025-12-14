using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record OtherDocumentNewParam
{
    public Guid OtherDocumentTypeId { get; init; }
    public string Name { get; init; } = default!;
    public string Year { get; init; } = default!;
    public Guid UserId { get; init; }
    public OtherDocumentStatus Status { get; init; }
    public string SAPCode { get; init; } = default!;
    public string LoanNumber { get; init; } = default!;
    public string CreatedBy { get; init; } = default!;
}
