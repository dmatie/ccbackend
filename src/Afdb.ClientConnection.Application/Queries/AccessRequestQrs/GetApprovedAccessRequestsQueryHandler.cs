using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

internal sealed class GetApprovedAccessRequestsQueryHandler
    : IRequestHandler<GetApprovedAccessRequestsQuery, PaginatedAccessRequestDto>
{
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly IMapper _mapper;

    public GetApprovedAccessRequestsQueryHandler(
        IAccessRequestRepository accessRequestRepository,
        IMapper mapper)
    {
        _accessRequestRepository = accessRequestRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedAccessRequestDto> Handle(
        GetApprovedAccessRequestsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PageNumber < 1)
        {
            throw new ValidationException(new ExceptionContent
            {
                Title = "Invalid Page Number",
                Message = "Page number must be greater than or equal to 1."
            });
        }

        if (request.PageSize < 1 || request.PageSize > 100)
        {
            throw new ValidationException(new ExceptionContent
            {
                Title = "Invalid Page Size",
                Message = "Page size must be between 1 and 100."
            });
        }

        var (items, totalCount) = await _accessRequestRepository.GetApprovedWithPaginationAsync(
            request.CountryId,
            request.ProjectCode,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var accessRequestDtos = _mapper.Map<List<AccessRequestDto>>(items);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PaginatedAccessRequestDto
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
