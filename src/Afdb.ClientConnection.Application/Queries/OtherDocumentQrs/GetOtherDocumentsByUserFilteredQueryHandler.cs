using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed class GetOtherDocumentsByUserFilteredQueryHandler
    : IRequestHandler<GetOtherDocumentsByUserFilteredQuery, GetOtherDocumentsByUserFilteredResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetOtherDocumentsByUserFilteredQueryHandler(
        IOtherDocumentRepository otherDocumentRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _otherDocumentRepository = otherDocumentRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<GetOtherDocumentsByUserFilteredResponse> Handle(
        GetOtherDocumentsByUserFilteredQuery request,
        CancellationToken cancellationToken)
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
        {
            throw new NotFoundException("ERR.General.UserNotFound");
        }

        var (otherDocuments, totalCount) = await _otherDocumentRepository.GetByUserIdWithFiltersAndPaginationAsync(
            user.Id,
            request.Status,
            request.OtherDocumentTypeId,
            request.SAPCode,
            request.Year,
            request.CreatedFrom,
            request.CreatedTo,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var otherDocumentDtos = otherDocuments != null && otherDocuments.Any()
            ? _mapper.Map<List<OtherDocumentDto>>(otherDocuments)
            : new List<OtherDocumentDto>();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return new GetOtherDocumentsByUserFilteredResponse
        {
            OtherDocuments = otherDocumentDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };
    }
}
