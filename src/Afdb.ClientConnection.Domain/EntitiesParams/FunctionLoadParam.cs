using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record FunctionLoadParam :  CommonLoadParam
{
    public string Code { get; init; }= default!;
    public string Name { get; init; }= default!;
    public string NameFr { get; init; }= default!;
    public string? Description { get; init; }= default!;
    public bool IsActive { get; init; }
}
