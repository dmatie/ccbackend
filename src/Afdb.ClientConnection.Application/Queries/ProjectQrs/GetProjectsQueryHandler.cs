using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;
public sealed class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, GetProjectsResponse>
{
    private readonly ISapService _sapService;

    public GetProjectsQueryHandler(ISapService sapService)
    {
        _sapService = sapService;
    }

    public async Task<GetProjectsResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = (await _sapService.GetProjectsAsync(cancellationToken)).ToList();

        return new GetProjectsResponse
        {
            Projects = projects,
            TotalCount = projects.Count,
            PageNumber = 1,
            PageSize = projects.Count,
            TotalPages = 1
        };
    }
}