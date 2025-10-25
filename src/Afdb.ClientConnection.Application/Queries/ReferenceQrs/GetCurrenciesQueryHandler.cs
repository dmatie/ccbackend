using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetCurrenciesQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetCurrenciesQuery, GetCurrenciesResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCurrenciesResponse> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await _referenceService.GetCurrenciesAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Function", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = currencies.Count();

        var dtos = _mapper.Map<List<CurrencyDto>>(currencies);

        return new GetCurrenciesResponse
        {
            Currencies = dtos,
            TotalCount = totalCount
        };
    }
}
