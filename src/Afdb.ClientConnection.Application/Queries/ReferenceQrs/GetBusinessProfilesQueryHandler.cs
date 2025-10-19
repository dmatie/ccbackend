using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using System.Diagnostics.Metrics;

namespace Afdb.ClientConnection.Application.Queries.ReferenceQrs;

public sealed class GetBusinessProfilesQueryHandler(IReferenceService referenceService, IMapper mapper) :
    IRequestHandler<GetBusinessProfilesQuery, GetBusinessProfilesResponse>
{
    private readonly IReferenceService _referenceService = referenceService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetBusinessProfilesResponse> Handle(GetBusinessProfilesQuery request, CancellationToken cancellationToken)
    {
        var businessProfiles = await _referenceService.GetActiveBusinessProfilesAsync(cancellationToken) ??
        throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("BusinessProfile", "ERR.General.ReferenceDataNotExist")
        });

        var totalCount = businessProfiles.Count();

        var dtos = _mapper.Map<List<BusinessProfileDto>>(businessProfiles);

        return new GetBusinessProfilesResponse
        {
            BusinessProfiles = dtos,
            TotalCount = totalCount
        };
    }
}
