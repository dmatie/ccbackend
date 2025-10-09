using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;

public sealed record GetProjectsQuery : IRequest<GetProjectsResponse>
{
    // Pas de param�tres d'entr�e
}

public sealed record GetProjectsResponse
{
    public List<ProjectDto> Projects { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}