using Afdb.ClientConnection.Domain.Entities;
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
    public User User { get; init; } = default!;
    public OtherDocumentType OtherDocumentType { get; init; } = default!;
    public string[] FileNames { get; init; } = [];
    public string[] AssignTo { get; init; } = [];
    public string[] AssignCc { get; init; } = [];
}
