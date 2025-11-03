using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record UserDto
{
    public Guid Id { get; init;}
    public string Email { get; init;} = string.Empty;
    public string FirstName { get; init;} = string.Empty;
    public string LastName { get; init;} = string.Empty;
    public string FullName { get; init;} = string.Empty;
    public UserRole Role { get; init;}
    public bool IsActive { get; init;}
    public string? EntraIdObjectId { get; init;}
    public string? OrganizationName { get; init;}
    public DateTime CreatedAt { get; init;}
    public List<CountryAdminDto>? Countries { get; init;} = [];
}
