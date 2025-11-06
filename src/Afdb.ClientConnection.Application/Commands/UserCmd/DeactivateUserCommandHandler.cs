using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeactivateUserCommandHandler(IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        // Vérifier que l'utilisateur actuel peut approuver
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null || !user.IsInternal)
            return false;

        user.Deactivate(_currentUserService.UserId);
        await _userRepository.UpdateAsync(user);
        return true;
    }
}