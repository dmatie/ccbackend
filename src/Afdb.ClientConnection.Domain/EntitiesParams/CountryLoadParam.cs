using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record CountryLoadParam: CommonLoadParam
{
    public string Name { get; init; } = default!;
    public string NameFr { get; init; } = default!;
    public string Code { get; init; }= default!;
    public bool IsActive { get; init; }
    public List<CountryAdmin> CountryAdmins { get; init; } = new();
}
