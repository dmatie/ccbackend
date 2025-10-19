using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserByEmailQuery : IRequest<GetUserByEmailResponse>
{
    public string Email { get; set; } = string.Empty;
}

public class GetUserByEmailResponse
{
    public UserDto? User { get; set; }
}