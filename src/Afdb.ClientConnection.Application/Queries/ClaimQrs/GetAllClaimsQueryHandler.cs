using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed class GetAllClaimsQueryHandler : IRequestHandler<GetAllClaimsQuery, GetAllClaimsResponse>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IUserContextService _userContextService;
    private readonly IMapper _mapper;

    public GetAllClaimsQueryHandler(
        IClaimRepository claimRepository,
        IUserContextService userContextService,
        IMapper mapper)
    {
        _claimRepository = claimRepository;
        _userContextService = userContextService;
        _mapper = mapper;
    }

    public async Task<GetAllClaimsResponse> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();

        var claims = request.Status.HasValue
            ? await _claimRepository.GetAllByStatusAsync(request.Status.Value)
            : await _claimRepository.GetAllAsync(userContext);


        List<ClaimDto> result = new();

        if (claims != null && claims.Any())
            result = _mapper.Map<IEnumerable<ClaimDto>>(claims).ToList();

        return new GetAllClaimsResponse
        {
            Claims = result,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = result.Count,
            TotalPages = (int)Math.Ceiling(result.Count / (double)request.PageSize)
        };
    }
}
