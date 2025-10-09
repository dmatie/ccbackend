using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;

public sealed record GetProjectsByCountryQuery(string CountryCode) : IRequest<GetProjectsByCountryResponse>;

public sealed record GetProjectsByCountryResponse
{
    public List<ProjectDto> Projects { get; init; } = [];
    public int TotalCount { get; init; }
}