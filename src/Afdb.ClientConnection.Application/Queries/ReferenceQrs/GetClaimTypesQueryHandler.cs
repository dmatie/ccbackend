using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Diagnostics.Metrics;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetClaimTypesQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetClaimTypesQuery, GetClaimTypesResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetClaimTypesResponse> Handle(GetClaimTypesQuery request, CancellationToken cancellationToken)
    {
        var functions = await _referenceService.GetActiveClaimTypesAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Function", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = functions.Count();

        var dtos = _mapper.Map<List<ClaimTypeDto>>(functions);

        return new GetClaimTypesResponse
        {
            ClaimTypes = dtos,
            TotalCount = totalCount
        };
    }
}
