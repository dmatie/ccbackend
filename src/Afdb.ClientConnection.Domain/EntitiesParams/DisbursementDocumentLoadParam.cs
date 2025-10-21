namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementDocumentLoadParam
{
    public required Guid Id { get; init; }
    public required Guid DisbursementId { get; init; }
    public required string FileName { get; init; }
    public required string DocumentUrl { get; init; }
    public required string CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
}
