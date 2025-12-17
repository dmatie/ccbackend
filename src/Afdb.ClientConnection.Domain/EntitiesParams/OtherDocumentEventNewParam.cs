using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record OtherDocumentEventNewParam
{
    public Guid OtherDocumentId { get; init; }
    public string DocumentName { get; init; } = default!;
    public string Year { get; init; } = default!;
    public string SapCode { get; init; } = default!;
    public string LoanNumber { get; init; } = default!;
    public OtherDocumentType OtherDocumentType { get; init; } = default!;
    public User CreatedByUser { get; init; } = default!;
    public string[] FileNames { get; init; } = [];
    public string[] AssignToEmail { get; init; } = [];
    public string[] AssignCcEmail { get; init; } = [];
}
