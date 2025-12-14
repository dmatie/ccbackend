namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record OtherDocumentTypeLoadParam : CommonLoadParam
{
    public string Name { get; init; } = default!;
    public string NameFr { get; init; } = default!;
    public bool IsActive { get; init; }
}
