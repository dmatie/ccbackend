using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed class GetOtherDocumentsWithFiltersQueryHandler
    : IRequestHandler<GetOtherDocumentsWithFiltersQuery, GetOtherDocumentsWithFiltersResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetOtherDocumentsWithFiltersQueryHandler(
        IOtherDocumentRepository otherDocumentRepository,
        IUserContextService userContextService,
        IMapper mapper)
    {
        _otherDocumentRepository = otherDocumentRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<GetOtherDocumentsWithFiltersResponse> Handle(
        GetOtherDocumentsWithFiltersQuery request,
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

        var (items, totalCount) = await _otherDocumentRepository.GetWithFiltersAndPaginationAsync(
            userContext,
            request.Status,
            request.OtherDocumentTypeId,
            request.SAPCode,
            request.Year,
            request.CreatedFrom,
            request.CreatedTo,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var otherDocumentDtos = _mapper.Map<List<OtherDocumentDto>>(items);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetOtherDocumentsWithFiltersResponse
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
