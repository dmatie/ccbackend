using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;

public sealed record GetProjectLoanNumberQuery(string sapCode) : IRequest<GetProjectLoanNumberResponse>;

public sealed record GetProjectLoanNumberResponse
{
    public List<ProjectLoanNumberDto> ProjectLoanNumbers { get; init; } = [];
    public int TotalCount { get; init; }
}