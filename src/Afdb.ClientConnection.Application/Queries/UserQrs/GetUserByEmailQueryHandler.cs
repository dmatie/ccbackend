using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<GetUserByEmailResponse> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        // Vérifier les permissions - Admin, DO, DA peuvent chercher par email
        if (!_currentUserService.IsInRole("Admin"))
        {
            // L'utilisateur peut chercher son propre profil
            if (_currentUserService.Email.ToLowerInvariant() != request.Email.ToLowerInvariant())
            {
                throw new UnauthorizedAccessException("ERR.General.NotAuthorize");
            }
        }

        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new NotFoundException("ERR.General.UserNotExist");

        var dto = _mapper.Map<UserDto>(user);

        return new GetUserByEmailResponse
        {
            User = dto
        };
    }
}
