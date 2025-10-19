using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;
using Afdb.ClientConnection.Application.Queries.AccessRequestQrs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessRequestsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Créer une nouvelle demande d'accès (accessible sans authentification)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<CreateAccessRequestResponse>> CreateAccessRequest(
        CreateAccessRequestCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAccessRequest), new { id = result.AccessRequest.Id }, result);
    }

    /// <summary>
    /// Modifier une demande d'accès qui a été rejété (accessible sans authentification)
    /// </summary>
    [HttpPatch]
    [AllowAnonymous]
    public async Task<ActionResult<UpdateRejectedAccessRequestResponse>> UpdateRejectedAccessRequest(
        UpdateRejectedAccessRequestCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAccessRequest), new { id = result.AccessRequest.Id }, result);
    }

    /// <summary>
    /// Générer un OTP pour un email donné (accessible sans authentification)
    /// </summary>
    [HttpPost("GenerateOtp")]
    [AllowAnonymous]
    public async Task<ActionResult> GenerateOtp(
        CreateOtpCommand command, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(command, cancellationToken);
        return Created();
    }

    /// <summary>
    /// Vérifie si un email possède une demande d'accès avec le statut "Rejected" (accessible sans authentification)
    /// </summary>
    [HttpGet("is-rejected")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> IsAccessRequestRejected([FromQuery] string email, 
        CancellationToken cancellationToken = default)
    {
        var query = new GetAccessRequestIsRejectedQuery
        {
            Email = email,
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Vérifie la validité d'un OTP pour un email (accessible sans authentification)
    /// </summary>
    [HttpPost("verify-otp")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> VerifyOtp([FromBody] VerifyOtpCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    /// <summary>
    /// Récupérer une demande d'accès par email (accessible sans authentification)
    /// </summary>
    [HttpGet("by-email")]
    [AllowAnonymous]
    public async Task<ActionResult<GetAccessRequestResponse>> GetAccessRequestByEmail([FromQuery] string email,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAccessRequestByEmailQuery { Email = email };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Récupérer une demande d'accès par ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetAccessRequestResponse>> GetAccessRequest(
        Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetAccessRequestQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister les demandes d'accès avec filtres (Admin/DO uniquement)
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "DOOrAdmin")]
    public async Task<ActionResult<GetAccessRequestsResponse>> GetAccessRequests(
        [FromQuery] GetAccessRequestsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Approuver une demande d'accès (Admin/DO uniquement)
    /// </summary>
    [HttpPost("{id}/approve")]
    [Authorize(Policy = "DOOrAdmin")]
    public async Task<ActionResult<ApproveAccessRequestResponse>> ApproveAccessRequest(
        Guid id, [FromBody] ApproveAccessRequestCommand command, CancellationToken cancellationToken = default)
    {
        command.AccessRequestId = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Rejeter une demande d'accès (Admin/DO uniquement)
    /// </summary>
    [HttpPost("{id}/reject")]
    [Authorize(Policy = "DOOrAdmin")]
    public async Task<ActionResult<RejectAccessRequestResponse>> RejectAccessRequest(
        Guid id, [FromBody] RejectAccessRequestCommand command, CancellationToken cancellationToken = default)
    {
        command.AccessRequestId = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}