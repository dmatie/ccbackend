using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public sealed record GetInternaUsersQuery : IRequest<IEnumerable<UserDto>>
{
}
