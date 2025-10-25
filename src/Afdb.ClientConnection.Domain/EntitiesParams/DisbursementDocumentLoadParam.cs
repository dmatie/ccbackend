namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementDocumentLoadParam : CommonLoadParam
{
    public required Guid DisbursementId { get; init; }
    public required string FileName { get; init; }
    public required string DocumentUrl { get; init; }
}
