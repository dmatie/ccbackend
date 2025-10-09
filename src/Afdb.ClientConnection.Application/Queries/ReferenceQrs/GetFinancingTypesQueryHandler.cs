using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Diagnostics.Metrics;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetFinancingTypesQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetFinancingTypesQuery, GetFinancingTypesResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetFinancingTypesResponse> Handle(GetFinancingTypesQuery request, CancellationToken cancellationToken)
    {
        var financingTypes = await _referenceService.GetActiveFinancingTypesAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("FinancingType", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = financingTypes.Count();

        var dtos = _mapper.Map<List<FinancingTypeDto>>(financingTypes);

        return new GetFinancingTypesResponse
        {
            FinancingTypes = dtos,
            TotalCount = totalCount
        };
    }
}
