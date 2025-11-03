using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed record CreateUserInternalUserCommand : IRequest<CreateUserInternalUserResponse>
{
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public List<Guid> CountryIds { get; init; } = [];
}

public sealed record CreateUserInternalUserResponse 
{
    public UserDto User { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
