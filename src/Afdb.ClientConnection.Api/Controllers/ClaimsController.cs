using Afdb.ClientConnection.Application.Commands.ClaimCmd;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.ClaimQrs;
using Afdb.ClientConnection.Domain.Enums;
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
    [HttpGet("by-user")]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ClaimDto>>> GetClaimsByUser(
        [FromQuery] GetClaimsByUserQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Récupérer les claims d'un utilisateur avec filtres et pagination (ExternalUser uniquement)
    /// </summary>
    /// <param name="status">Filtre par statut du claim</param>
    /// <param name="claimTypeId">Filtre par type de claim</param>
    /// <param name="countryId">Filtre par pays</param>
    /// <param name="createdFrom">Date de début de la plage de création</param>
    /// <param name="createdTo">Date de fin de la plage de création</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 10, max: 100)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste paginée des claims de l'utilisateur avec métadonnées</returns>
    [HttpGet("by-user-filtered")]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetClaimsByUserFilteredResponse>> GetClaimsByUserFiltered(
        [FromQuery] ClaimStatus? status,
        [FromQuery] Guid? claimTypeId,
        [FromQuery] Guid? countryId,
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetClaimsByUserFilteredQuery
        {
            Status = status,
            ClaimTypeId = claimTypeId,
            CountryId = countryId,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

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
    /// Récupérer la liste des claims avec filtres et pagination (Admin, DA, DO uniquement)
    /// </summary>
    /// <param name="status">Filtre par statut du claim</param>
    /// <param name="claimTypeId">Filtre par type de claim</param>
    /// <param name="countryId">Filtre par pays</param>
    /// <param name="createdFrom">Date de début de la plage de création</param>
    /// <param name="createdTo">Date de fin de la plage de création</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 10, max: 100)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste paginée des claims avec métadonnées</returns>
    [HttpGet("with-filters")]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GetClaimsWithFiltersResponse>> GetClaimsWithFilters(
        [FromQuery] ClaimStatus? status,
        [FromQuery] Guid? claimTypeId,
        [FromQuery] Guid? countryId,
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetClaimsWithFiltersQuery
        {
            Status = status,
            ClaimTypeId = claimTypeId,
            CountryId = countryId,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

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
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetClaimById), new { id }, result);
    }
}
