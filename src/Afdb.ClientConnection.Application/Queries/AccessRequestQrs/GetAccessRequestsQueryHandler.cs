using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed class GetAccessRequestsQueryHandler(
    IAccessRequestRepository accessRequestRepository,
    ICurrentUserService currentUserService,
    IUserContextService userContextService,
    IMapper mapper) : IRequestHandler<GetAccessRequestsQuery, GetAccessRequestsResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;
    private readonly IUserContextService _userContextService = userContextService;

    public async Task<GetAccessRequestsResponse> Handle(GetAccessRequestsQuery request, CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();

        // Seuls les Admin et DO peuvent lister les demandes
        if (!_currentUserService.IsInRole("Admin") && !_currentUserService.IsInRole("DO"))
        {
            throw new ForbiddenAccessException("ERR.General.Forbidden");
        }

        IEnumerable<Domain.Entities.AccessRequest> accessRequests;

        // Filtrer par statut si spécifié
        if (request.Status.HasValue)
        {
            accessRequests = await _accessRequestRepository.GetByStatusAsync(userContext, request.Status.Value);
        }
        else
        {
            accessRequests = await _accessRequestRepository.GetAllAsync(userContext);
        }

        // Filtrer par email si spécifié
        if (!string.IsNullOrEmpty(request.Email))
        {
            accessRequests = accessRequests.Where(ar =>
                ar.Email.Contains(request.Email, StringComparison.OrdinalIgnoreCase));
        }

        // Filtrer par date si spécifiée
        if (request.FromDate.HasValue)
        {
            accessRequests = accessRequests.Where(ar => ar.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            accessRequests = accessRequests.Where(ar => ar.CreatedAt <= request.ToDate.Value);
        }

        // Ordonner par date de création (plus récent en premier)
        accessRequests = accessRequests.OrderByDescending(ar => ar.CreatedAt);

        // Pagination
        var totalCount = accessRequests.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var pagedRequests = accessRequests
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<AccessRequestDto>>(pagedRequests);

        return new GetAccessRequestsResponse
        {
            AccessRequests = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
}