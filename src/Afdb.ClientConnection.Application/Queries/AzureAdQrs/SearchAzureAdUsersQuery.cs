using Afdb.ClientConnection.Application.DTOs;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AzureAdQrs;

public sealed record SearchAzureAdUsersQuery : IRequest<SearchAzureAdUsersResponse>
{
    public string SearchQuery { get; init; } = string.Empty;
    public int MaxResults { get; init; } = 10;
}
public sealed record SearchAzureAdUsersResponse
{
    public List<AzureAdUserDetailsDto> Users { get; init; } = [];
    public int TotalCount { get; init; }
}

