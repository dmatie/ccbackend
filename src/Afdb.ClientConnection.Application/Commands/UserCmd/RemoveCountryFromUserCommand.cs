using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;


public sealed record RemoveCountryFromUserCommand(Guid UserId, Guid CountryId) : IRequest<bool>;