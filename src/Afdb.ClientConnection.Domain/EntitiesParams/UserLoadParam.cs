using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record UserLoadParam : CommonLoadParam
{
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; }= default!;
    public UserRole Role { get; init; }
    public bool IsActive { get; init; }
    public string? EntraIdObjectId { get; init; } // Azure AD Object ID
    public string? OrganizationName { get; init; }
    public List<CountryAdmin>? Countries { get; init; } = [];
}
