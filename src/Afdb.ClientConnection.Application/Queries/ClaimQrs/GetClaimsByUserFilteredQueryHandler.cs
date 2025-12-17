using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

public sealed class GetClaimsByUserFilteredQueryHandler : IRequestHandler<GetClaimsByUserFilteredQuery, GetClaimsByUserFilteredResponse>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetClaimsByUserFilteredQueryHandler(
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

    public async Task<GetClaimsByUserFilteredResponse> Handle(GetClaimsByUserFilteredQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber < 1)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("PageNumber", "ERR.General.PageNumberGEOne")
            });
        }

        if (request.PageSize < 1 || request.PageSize > 100)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("PageSize", "ERR.General.PageSizeInterval")
            });
        }

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email);
        if (user == null)
            throw new NotFoundException("User", _currentUserService.Email);

        var (claims, totalCount) = await _claimRepository.GetByUserIdWithFiltersAndPaginationAsync(
            user.Id,
            request.Status,
            request.ClaimTypeId,
            request.CountryId,
            request.CreatedFrom,
            request.CreatedTo,
            request.PageNumber,
            request.PageSize);

        var claimDtos = claims != null && claims.Any()
            ? _mapper.Map<List<ClaimDto>>(claims)
            : new List<ClaimDto>();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new GetClaimsByUserFilteredResponse
        {
            Claims = claimDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };
    }
}
