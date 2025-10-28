using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.DashboardQrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("internal-stats")]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<InternalDashboardStatsDto>> GetInternalDashboardStats(
        CancellationToken cancellationToken = default)
    {
        var query = new GetInternalDashboardStatsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("external-stats")]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExternalDashboardStatsDto>> GetExternalDashboardStats(
        CancellationToken cancellationToken = default)
    {
        var query = new GetExternalDashboardStatsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
