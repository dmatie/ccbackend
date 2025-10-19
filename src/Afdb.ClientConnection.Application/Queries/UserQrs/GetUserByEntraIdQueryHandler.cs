using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserByEntraIdQueryHandler : IRequestHandler<GetUserByEntraIdQuery, GetUserByEntraIdResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserByEntraIdQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<GetUserByEntraIdResponse> Handle(GetUserByEntraIdQuery request, CancellationToken cancellationToken)
    {
        // Vérifier les permissions - Admin, DO, DA peuvent chercher par Entra ID
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new UnauthorizedAccessException("ERR.General.NotAuthorize");
        }

        var user = await _userRepository.GetByEntraIdObjectIdAsync(request.EntraIdObjectId);

        var dto = user != null ? _mapper.Map<UserDto>(user) : null;

        return new GetUserByEntraIdResponse
        {
            User = dto
        };
    }
}