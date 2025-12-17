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
    /// <param name="query">Paramètre de Filtre</param>
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
        [FromQuery] GetOtherDocumentsByUserFilteredQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Récupérer la liste des documents avec filtres et pagination (Admin, DA, DO uniquement)
    /// </summary>
    /// <param name="query">Filtre</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste paginée des documents avec métadonnées</returns>
    [HttpGet("with-filters")]
    [Authorize(Policy = "InternalUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GetOtherDocumentsWithFiltersResponse>> GetOtherDocumentsWithFilters(
        [FromQuery] GetOtherDocumentsWithFiltersQuery  query,
        CancellationToken cancellationToken = default)
    {
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

    /// <summary>
    /// Télécharger le premier fichier d'un document (Tous les rôles authentifiés)
    /// </summary>
    /// <param name="otherDocumentId">ID du document</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Le premier fichier du document</returns>
    [HttpGet("DownloadFile")]
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
}
