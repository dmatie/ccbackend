using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public sealed class GetInternaUsersQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetInternaUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UserDto>> Handle(GetInternaUsersQuery request, CancellationToken cancellationToken)
    {
        // Seuls les Admin peuvent lister tous les utilisateurs
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new UnauthorizedAccessException("ERR.General.NotAuthorize");
        }

        var users = await _userRepository.GetActiveInternalUsersAsync();

        var dtos = _mapper.Map<List<UserDto>>(users);

        return dtos;
    }
}