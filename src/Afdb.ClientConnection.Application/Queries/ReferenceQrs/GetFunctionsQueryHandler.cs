using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Diagnostics.Metrics;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetFunctionsQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetFunctionsQuery, GetFunctionsResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetFunctionsResponse> Handle(GetFunctionsQuery request, CancellationToken cancellationToken)
    {
        var functions = await _referenceService.GetActiveFunctionsAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Function", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = functions.Count();

        var dtos = _mapper.Map<List<FunctionDto>>(functions);

        return new GetFunctionsResponse
        {
            Functions = dtos,
            TotalCount = totalCount
        };
    }
}
