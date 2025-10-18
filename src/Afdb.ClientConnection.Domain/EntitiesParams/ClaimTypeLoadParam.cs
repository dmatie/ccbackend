using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record ClaimTypeLoadParam :  CommonLoadParam
{
    public string Name { get; init; }= default!;
    public string NameFr { get; init; }= default!;
    public string Description { get; init; }= default!;
}
