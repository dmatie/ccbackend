using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record CountryAdminLoadParam : CommonLoadParam
{
    public Guid CountryId { get; init; }
    public Guid UserId { get; init; } 
    public bool IsActive { get; init; }
    public User User { get; set; } = default!;
    public Country Country { get; set; } = default!;
}
