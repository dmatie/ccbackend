using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserByEntraIdQuery : IRequest<GetUserByEntraIdResponse>
{
    public string EntraIdObjectId { get; set; } = string.Empty;
}

public class GetUserByEntraIdResponse
{
    public UserDto? User { get; set; }
}