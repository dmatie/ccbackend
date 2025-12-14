using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record OtherDocumentLoadParam : CommonLoadParam
{
    public Guid OtherDocumentTypeId { get; init; }
    public string Name { get; init; } = default!;
    public string Year { get; init; } = default!;
    public Guid UserId { get; init; }
    public OtherDocumentStatus Status { get; init; }
    public string SAPCode { get; init; } = default!;
    public string LoanNumber { get; init; } = default!;
    public OtherDocumentType? OtherDocumentType { get; init; }
    public User? User { get; init; }
    public List<OtherDocumentFile> Files { get; init; } = [];
}
