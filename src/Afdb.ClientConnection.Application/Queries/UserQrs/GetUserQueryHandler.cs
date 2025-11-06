using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public class GetUserQueryHandler(
    IUserRepository userRepository,
    IMapper mapper) : IRequestHandler<GetUserQuery, GetUserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("ERR.General.UserNotExist");

        var dto = _mapper.Map<UserDto>(user);

        return new GetUserResponse
        {
            User = dto
        };
    }
}
