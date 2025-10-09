using Afdb.ClientConnection.Application.Commands.UserCmd;
using Afdb.ClientConnection.Application.Queries.UserQrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Afdb.ClientConnection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Lister les utilisateurs avec filtres (Admin uniquement)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetUsersResponse>> GetUsers(
        [FromQuery] GetUsersQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Créer un nouvel utilisateur (Admin/DO uniquement)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "DOOrAdmin")]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(
        CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUserByEmail), new { email = result.User.Email }, result);
    }

    /// <summary>
    /// Récupérer un utilisateur par email (Admin utilisateur lui-même)
    /// </summary>
    [HttpGet("by-email/{email}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<GetUserByEmailResponse>> GetUserByEmail(
        string email, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByEmailQuery { Email = email };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.User == null)
        {
            return NotFound($"Aucun utilisateur trouvé avec l'email: {email}");
        }

        return Ok(result);
    }

    /// <summary>
    /// Récupérer un utilisateur par Entra ID (Admin uniquement)
    /// </summary>
    [HttpGet("by-entraid/{entraIdObjectId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<GetUserByEntraIdResponse>> GetUserByEntraId(
        string entraIdObjectId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByEntraIdQuery { EntraIdObjectId = entraIdObjectId };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.User == null)
        {
            return NotFound($"Aucun utilisateur trouvé avec l'Entra ID: {entraIdObjectId}");
        }

        return Ok(result);
    }
}