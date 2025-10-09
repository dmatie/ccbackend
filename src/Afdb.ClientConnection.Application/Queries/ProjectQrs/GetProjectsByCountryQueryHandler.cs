using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;

public sealed class GetProjectsByCountryQueryHandler : IRequestHandler<GetProjectsByCountryQuery, GetProjectsByCountryResponse>
{
    private readonly ISapService _sapService;

    public GetProjectsByCountryQueryHandler(ISapService sapService)
    {
        _sapService = sapService;
    }

    public async Task<GetProjectsByCountryResponse> Handle(GetProjectsByCountryQuery request, CancellationToken cancellationToken)
    {
        var projects = (await _sapService.GetProjectsByCountryAsync(request.CountryCode, cancellationToken)).ToList();
        return new GetProjectsByCountryResponse
        {
            Projects = projects,
            TotalCount = projects.Count
        };
    }
}