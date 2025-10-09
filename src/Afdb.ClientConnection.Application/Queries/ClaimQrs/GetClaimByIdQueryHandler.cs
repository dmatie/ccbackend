using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, ClaimDto>
{
    private readonly IClaimRepository _claimRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetClaimByIdQueryHandler(
        IClaimRepository claimRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _claimRepository = claimRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ClaimDto> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
    {
        var claim = await _claimRepository.GetByIdAsync(request.ClaimId);
        
        if (claim == null)
            throw new NotFoundException("Claim", request.ClaimId);

        if (claim.User?.Email != _currentUserService.Email)
            throw new ForbiddenAccessException("You can only access your own claims");

        return _mapper.Map<ClaimDto>(claim);
    }
}
