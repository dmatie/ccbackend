using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;

public sealed class GetProjectsByCountryQueryHandler(ISapService sapService, IMapper mapper) :
    IRequestHandler<GetProjectsByCountryQuery, GetProjectsByCountryResponse>
{
    private readonly ISapService _sapService = sapService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetProjectsByCountryResponse> Handle(GetProjectsByCountryQuery request, CancellationToken cancellationToken)
    {
        var projects = (await _sapService.GetProjectsByCountryAsync(request.CountryCode, cancellationToken)).ToList();

        List<ProjectDto> dtos = _mapper.Map<List<ProjectDto>>(projects);

        return new GetProjectsByCountryResponse
        {
            Projects = dtos,
            TotalCount = projects.Count
        };
    }
}