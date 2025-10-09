using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed record CreateUserCommand : IRequest<CreateUserResponse>
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? EntraIdObjectId { get; set; }
    public string? OrganizationName { get; set; }
}

public sealed record CreateUserResponse
{
    public UserDto User { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
