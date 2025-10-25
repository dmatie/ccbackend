namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementDocumentNewParam
{
    public required Guid DisbursementId { get; init; }
    public required string FileName { get; init; } = null!;
    public required string DocumentUrl { get; init; } = null!;
    public required string CreatedBy { get; init; } = null!;
}
