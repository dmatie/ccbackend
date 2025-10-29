using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Queries.AzureAdQrs;

public sealed class SearchAzureAdUsersQueryHandler : IRequestHandler<SearchAzureAdUsersQuery, SearchAzureAdUsersResponse>
{
    private readonly IGraphService _graphService;
    private readonly ILogger<SearchAzureAdUsersQueryHandler> _logger;

    public SearchAzureAdUsersQueryHandler(
        IGraphService graphService,
        ILogger<SearchAzureAdUsersQueryHandler> logger)
    {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<SearchAzureAdUsersResponse> Handle(SearchAzureAdUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling SearchAzureAdUsersQuery with SearchQuery: {SearchQuery}, MaxResults: {MaxResults}",
            request.SearchQuery,
            request.MaxResults);

        if (string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            _logger.LogWarning("Search query is empty or null");
            return new SearchAzureAdUsersResponse
            {
                Users = [],
                TotalCount = 0
            };
        }

        var users = await _graphService.SearchUsersAsync(
            request.SearchQuery,
            request.MaxResults,
            cancellationToken);

        _logger.LogInformation(
            "SearchAzureAdUsersQuery completed. Found {Count} users for query: {SearchQuery}",
            users.Count,
            request.SearchQuery);

        return new SearchAzureAdUsersResponse
        {
            Users = users,
            TotalCount = users.Count
        };
    }
}
