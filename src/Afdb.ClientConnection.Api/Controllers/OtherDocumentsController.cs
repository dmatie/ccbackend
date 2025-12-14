using Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OtherDocumentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Créer un nouveau document (ExternalUser uniquement)
    /// </summary>
    /// <param name="command">Données du document à créer</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Le document créé avec son ID</returns>
    [HttpPost]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CreateOtherDocumentResponse>> CreateOtherDocument(
        [FromForm] CreateOtherDocumentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOtherDocumentById), new { id = result.OtherDocument.Id }, result);
    }

    /// <summary>
    /// Récupérer les documents d'un utilisateur avec filtres et pagination (ExternalUser uniquement)
    /// </summary>
    /// <param name="status">Filtre par statut du document</param>
    /// <param name="otherDocumentTypeId">Filtre par type de document</param>
    /// <param name="sapCode">Filtre par code SAP</param>
    /// <param name="year">Filtre par année</param>
    /// <param name="createdFrom">Date de début de la plage de création</param>
    /// <param name="createdTo">Date de fin de la plage de création</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 10, max: 100)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste paginée des documents de l'utilisateur avec métadonnées</returns>
    [HttpGet("by-user-filtered")]
    [Authorize(Policy = "ExternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetOtherDocumentsByUserFilteredResponse>> GetOtherDocumentsByUserFiltered(
        [FromQuery] OtherDocumentStatus? status,
        [FromQuery] Guid? otherDocumentTypeId,
        [FromQuery] string? sapCode,
        [FromQuery] string? year,
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOtherDocumentsByUserFilteredQuery
        {
            Status = status,
            OtherDocumentTypeId = otherDocumentTypeId,
            SAPCode = sapCode,
            Year = year,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Récupérer la liste des documents avec filtres et pagination (Admin, DA, DO uniquement)
    /// </summary>
    /// <param name="status">Filtre par statut du document</param>
    /// <param name="otherDocumentTypeId">Filtre par type de document</param>
    /// <param name="sapCode">Filtre par code SAP</param>
    /// <param name="year">Filtre par année</param>
    /// <param name="createdFrom">Date de début de la plage de création</param>
    /// <param name="createdTo">Date de fin de la plage de création</param>
    /// <param name="pageNumber">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 10, max: 100)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste paginée des documents avec métadonnées</returns>
    [HttpGet("with-filters")]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GetOtherDocumentsWithFiltersResponse>> GetOtherDocumentsWithFilters(
        [FromQuery] OtherDocumentStatus? status,
        [FromQuery] Guid? otherDocumentTypeId,
        [FromQuery] string? sapCode,
        [FromQuery] string? year,
        [FromQuery] DateTime? createdFrom,
        [FromQuery] DateTime? createdTo,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOtherDocumentsWithFiltersQuery
        {
            Status = status,
            OtherDocumentTypeId = otherDocumentTypeId,
            SAPCode = sapCode,
            Year = year,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Récupérer un document par son ID (Tous les rôles authentifiés)
    /// </summary>
    /// <param name="id">ID du document</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Le document avec ses détails</returns>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OtherDocumentDto>> GetOtherDocumentById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOtherDocumentByIdQuery { OtherDocumentId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Télécharger le premier fichier d'un document (Tous les rôles authentifiés)
    /// </summary>
    /// <param name="otherDocumentId">ID du document</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Le premier fichier du document</returns>
    [HttpGet("DownloadFirstFile")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadFirstFile(
        [FromQuery] Guid otherDocumentId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFirstFileQuery { OtherDocumentId = otherDocumentId };
        var result = await _mediator.Send(query, cancellationToken);
        return File(result.FileContent, result.ContentType, result.FileName);
    }

    /// <summary>
    /// Récupérer la liste des types de documents (Tous les rôles authentifiés)
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste des types de documents actifs</returns>
    [HttpGet("types")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<OtherDocumentTypeDto>>> GetOtherDocumentTypes(
        CancellationToken cancellationToken = default)
    {
        var query = new GetOtherDocumentTypesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
