using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ClaimQrs;

internal sealed class GetClaimsWithFiltersQueryHandler
    : IRequestHandler<GetClaimsWithFiltersQuery, GetClaimsWithFiltersResponse>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetClaimsWithFiltersQueryHandler(
        IClaimRepository claimRepository,
        IUserContextService userContextService,
        IMapper mapper)
    {
        _claimRepository = claimRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<GetClaimsWithFiltersResponse> Handle(
        GetClaimsWithFiltersQuery request,
        CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();

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

        var (items, totalCount) = await _claimRepository.GetWithFiltersAndPaginationAsync(
            userContext,
            request.Status,
            request.ClaimTypeId,
            request.CountryId,
            request.CreatedFrom,
            request.CreatedTo,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var claimDtos = _mapper.Map<List<ClaimDto>>(items);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetClaimsWithFiltersResponse
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
