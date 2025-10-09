using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed class GetClaimsByUserQueryHandler : IRequestHandler<GetClaimsByUserQuery, IEnumerable<ClaimDto>>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetClaimsByUserQueryHandler(
        IClaimRepository claimRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _claimRepository = claimRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClaimDto>> Handle(GetClaimsByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        if (user.Email != _currentUserService.Email)
            throw new ForbiddenAccessException("You can only access your own claims");

        var claims = await _claimRepository.GetByUserIdAndStatusAsync(request.UserId, request.Status);

        if (claims == null || !claims.Any())
            return [];

        return _mapper.Map<IEnumerable<ClaimDto>>(claims);
    }
}
