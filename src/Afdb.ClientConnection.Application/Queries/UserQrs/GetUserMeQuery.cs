using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserMeQuery : IRequest<GetUserMeResponse>
{
    public string Email { get; set; } = string.Empty;
}

public class GetUserMeResponse
{
    public UserDto? User { get; set; }
}