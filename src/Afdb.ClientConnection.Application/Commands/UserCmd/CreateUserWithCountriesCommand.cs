using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed record CreateUserWithCountriesCommand : IRequest<UserWithCountriesDto>
{
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public List<Guid> CountryIds { get; init; } = [];
}
