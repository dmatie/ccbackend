namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record CountryAdminNewParam
{
    public Guid CountryId { get; init; }
    public Guid UserId { get; init; } 
    public bool IsActive { get; init; } = true;
}
