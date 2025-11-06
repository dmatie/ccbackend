using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;


public sealed record AddCountryToUserCommand(Guid UserId, List<Guid> CountryIds) : IRequest<bool>;