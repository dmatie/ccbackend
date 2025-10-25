using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetDisbursementTypesQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetDisbursementTypesQuery, GetDisbursementTypesResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDisbursementTypesResponse> Handle(GetDisbursementTypesQuery request, CancellationToken cancellationToken)
    {
        var disbursementTypes = await _referenceService.GetDisbursmentTypesAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Function", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = disbursementTypes.Count();

        var dtos = _mapper.Map<List<DisbursementTypeDto>>(disbursementTypes);

        return new GetDisbursementTypesResponse
        {
            DisbursementTypes = dtos,
            TotalCount = totalCount
        };
    }
}
