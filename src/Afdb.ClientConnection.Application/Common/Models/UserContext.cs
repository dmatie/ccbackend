using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Models;

public sealed record UserContext
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public List<Guid> CountryIds { get; init; } = [];

    public bool IsAdmin => Role == UserRole.Admin;
    public bool IsExternal => Role == UserRole.ExternalUser;
    public bool RequiresCountryFilter => Role == UserRole.DO || Role == UserRole.DA;
}
