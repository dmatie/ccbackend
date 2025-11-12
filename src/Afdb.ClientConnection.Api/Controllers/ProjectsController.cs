using Afdb.ClientConnection.Application.Queries.ProjectQrs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("country/{countryCode}")]
    public async Task<ActionResult<GetProjectsByCountryResponse>> GetProjectsByCountry(string countryCode, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetProjectsByCountryQuery(countryCode), cancellationToken);
        return Ok(response);
    }
}