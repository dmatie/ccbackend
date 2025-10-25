using Afdb.ClientConnection.Application.Queries.ReferenceQrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferencesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Lister les pays actifs
    /// </summary>
    [HttpGet("countries")]
    [AllowAnonymous]
    public async Task<ActionResult<GetCountriesResponse>> GetCountries(CancellationToken cancellationToken = default)
    {
        var query = new GetCountriesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les fonctions actifs
    /// </summary>
    [HttpGet("functions")]
    [AllowAnonymous]
    public async Task<ActionResult<GetFunctionsResponse>> GetFunctions(CancellationToken cancellationToken = default)
    {
        var query = new GetFunctionsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les BusinessProfile actifs
    /// </summary>
    [HttpGet("business-profiles")]
    [AllowAnonymous]
    public async Task<ActionResult<GetBusinessProfilesResponse>> GetBusinessProfiles(CancellationToken cancellationToken = default)
    {
        var query = new GetBusinessProfilesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les Financing types actifs
    /// </summary>
    [HttpGet("financing-types")]
    [AllowAnonymous]
    public async Task<ActionResult<GetFinancingTypesResponse>> GetFinancingTypes(CancellationToken cancellationToken = default)
    {
        var query = new GetFinancingTypesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les Claim types actifs
    /// </summary>
    [HttpGet("claim-types")]
    [Authorize]
    public async Task<ActionResult<GetClaimTypesResponse>> GetClaimTypes(CancellationToken cancellationToken = default)
    {
        var query = new GetClaimTypesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les Disbursement type actifs
    /// </summary>
    [HttpGet("disbursement-types")]
    [Authorize]
    public async Task<ActionResult<GetDisbursementTypesResponse>> GetDisbursementTypes(CancellationToken cancellationToken = default)
    {
        var query = new GetDisbursementTypesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les currencies
    /// </summary>
    [HttpGet("currencies")]
    [Authorize]
    public async Task<ActionResult<GetCurrenciesResponse>> GetCurrencies(CancellationToken cancellationToken = default)
    {
        var query = new GetCurrenciesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
