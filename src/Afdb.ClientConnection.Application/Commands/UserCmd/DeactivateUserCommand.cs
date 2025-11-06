using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;
public sealed record DeactivateUserCommand(Guid UserId) : IRequest<bool>; 