using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.AzureAdQrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AzureAdController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("users/search")]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<SearchAzureAdUsersResponse>> SearchUsers(
        [FromQuery] string query,
        [FromQuery] int maxResults = 10,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new { error = "Search query is required" });
        }

        if (maxResults < 1 || maxResults > 50)
        {
            return BadRequest(new { error = "MaxResults must be between 1 and 50" });
        }

        var searchQuery = new SearchAzureAdUsersQuery
        {
            SearchQuery = query,
            MaxResults = maxResults
        };

        var result = await _mediator.Send(searchQuery, cancellationToken);
        return Ok(result);
    }
}
