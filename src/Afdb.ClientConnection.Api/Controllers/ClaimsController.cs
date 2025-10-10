using Afdb.ClientConnection.Application.Commands.ClaimCmd;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.ClaimQrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Créer un nouveau claim (ExternalUser uniquement)
    /// </summary>
    /// <param name="command">Données du claim à créer</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Le claim créé avec son ID</returns>
    [HttpPost]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CreateClaimResponse>> CreateClaim(
        [FromBody] CreateClaimCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetClaimById), new { id = result.Claim.Id }, result);
    }

    /// <summary>
    /// Récupérer tous les claims d'un utilisateur (ExternalUser uniquement)
    /// </summary>
    /// <param name="userId">ID de l'utilisateur</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste des claims de l'utilisateur</returns>
    [HttpGet("user/{userId}")]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ClaimDto>>> GetClaimsByUser(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetClaimsByUserQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Récupérer un claim par son ID (Tous les rôles authentifiés)
    /// </summary>
    /// <param name="id">ID du claim</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Le claim avec ses détails</returns>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClaimDto>> GetClaimById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetClaimByIdQuery { ClaimId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Lister tous les claims avec filtres optionnels (Admin, DA, DO uniquement)
    /// </summary>
    /// <param name="query">Paramètres de requête (filtres, pagination)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste paginée des claims</returns>
    [HttpGet]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<ClaimDto>>> GetAllClaims(
        [FromQuery] GetAllClaimsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Ajouter une réponse à un claim (Admin, DA, DO uniquement)
    /// </summary>
    /// <param name="id">ID du claim</param>
    /// <param name="command">Données de la réponse</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>La réponse créée</returns>
    [HttpPost("{id}/responses")]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClaimDto>> AddClaimResponse(
        Guid id,
        [FromBody] AddClaimResponseCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ClaimId = id;
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetClaimById), new { id }, result);
    }
}
