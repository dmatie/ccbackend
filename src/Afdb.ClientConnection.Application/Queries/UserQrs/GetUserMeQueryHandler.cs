using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserMeQueryHandler(
    IUserRepository userRepository,
    IGraphService graphService,
    IMapper mapper) : IRequestHandler<GetUserMeQuery, GetUserMeResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUserMeResponse> Handle(GetUserMeQuery request, CancellationToken cancellationToken)
    {
        // Vérifier si l'utilisateur existe dans Entra ID
        var userExistsInEntraId = await _graphService.UserExistsAsync(request.Email, cancellationToken);
        if (!userExistsInEntraId)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.General.EmailExistsInEntra")
            });
        }

        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new NotFoundException("ERR.General.UserNotExist");

        var dto = _mapper.Map<UserDto>(user);

        return new GetUserMeResponse
        {
            User = dto
        };
    }
}
