using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

internal sealed class GetApprovedAccessRequestsQueryHandler
    : IRequestHandler<GetApprovedAccessRequestsQuery, GetApprovedAccessRequestsResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetApprovedAccessRequestsQueryHandler(
        IAccessRequestRepository accessRequestRepository,
        IUserContextService userContextService,
        IMapper mapper)
    {
        _accessRequestRepository = accessRequestRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<GetApprovedAccessRequestsResponse> Handle(
        GetApprovedAccessRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();


        if (request.PageNumber < 1)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.General.PageNumberGEOne")
            });
        }

        if (request.PageSize < 1 || request.PageSize > 100)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.General.PageSizeInterval")
            });
        }

        var (items, totalCount) = await _accessRequestRepository.GetApprovedWithPaginationAsync(
            userContext,
            request.CountryId,
            request.ProjectCode,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var accessRequestDtos = _mapper.Map<List<AccessRequestDto>>(items);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetApprovedAccessRequestsResponse
        {
            AccessRequests = accessRequestDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };
    }
}
