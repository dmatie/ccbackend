using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed class GetAllClaimsQueryHandler : IRequestHandler<GetAllClaimsQuery, IEnumerable<ClaimDto>>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IMapper _mapper;

    public GetAllClaimsQueryHandler(
        IClaimRepository claimRepository,
        IMapper mapper)
    {
        _claimRepository = claimRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClaimDto>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
    {
        var claims = request.Status.HasValue
            ? await _claimRepository.GetAllByStatusAsync(request.Status.Value)
            : await _claimRepository.GetAllAsync();

        if (claims == null || !claims.Any())
            return [];

        return _mapper.Map<IEnumerable<ClaimDto>>(claims);
    }
}
