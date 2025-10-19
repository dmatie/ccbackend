using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed class GetClaimsByUserQueryHandler : IRequestHandler<GetClaimsByUserQuery, GetClaimsByUserResponse>
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

    public async Task<GetClaimsByUserResponse> Handle(GetClaimsByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email);
        if (user == null)
            throw new NotFoundException("User", _currentUserService.Email);


        var claims = await _claimRepository.GetByUserIdAndStatusAsync(user.Id, request.Status);

        List<ClaimDto> claimDtos = [];

        if (claims != null && claims.Any())
            claimDtos= _mapper.Map<IEnumerable<ClaimDto>>(claims).ToList();

        var totalCount = claimDtos.Count;
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);


        return new GetClaimsByUserResponse
        {
            Claims = claimDtos,
            TotalCount = claimDtos.Count,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages= totalPages
        };
    }
}
