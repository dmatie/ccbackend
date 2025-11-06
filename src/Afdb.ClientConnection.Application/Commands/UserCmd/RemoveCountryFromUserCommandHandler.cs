using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;
public sealed class RemoveCountryFromUserCommandHandler : IRequestHandler<RemoveCountryFromUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;


    public RemoveCountryFromUserCommandHandler(IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveCountryFromUserCommand request, CancellationToken cancellationToken)
    {

        // Vérifier que l'utilisateur actuel peut approuver
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("UserId", "ERR.User.NotFound")
                });

        // Suppression du pays (CountryAdmin) par son Id

        var countryToRemove = user.Countries.FirstOrDefault(ca => ca.CountryId == request.CountryId);
        if (countryToRemove == null)
            throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("UserId", "ERR.User.CountryNotAssigned")
                });

        user.RemoveCountry(countryToRemove.Id, _currentUserService.UserId ?? "System");
        await _userRepository.UpdateAsync(user);

        return true;
    }
}