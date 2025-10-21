using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Diagnostics.Metrics;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetCountriesQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetCountriesQuery, GetCountriesResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCountriesResponse> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _referenceService.GetActiveCountriesAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Country", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = countries.Count();

        var dtos = _mapper.Map<List<CountryDto>>(countries);

        return new GetCountriesResponse
        {
            Countries = dtos,
            TotalCount = totalCount
        };
    }
}
