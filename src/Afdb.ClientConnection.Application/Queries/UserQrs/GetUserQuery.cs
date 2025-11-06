using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserQuery : IRequest<GetUserResponse>
{
    public Guid Id { get; set; } 
}

public class GetUserResponse
{
    public UserDto? User { get; set; }
}