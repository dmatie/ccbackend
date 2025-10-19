using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class ApproveAccessRequestCommandHandler(
    IAccessRequestRepository accessRequestRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IAuditService auditService,
    IMapper mapper, 
    IGraphService graphService,
    ILogger<ApproveAccessRequestCommandHandler> logger) : IRequestHandler<ApproveAccessRequestCommand, ApproveAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;
    private readonly IGraphService _graphService = graphService;
    private readonly ILogger<ApproveAccessRequestCommandHandler> _logger = logger;

    public async Task<ApproveAccessRequestResponse> Handle(ApproveAccessRequestCommand request, CancellationToken cancellationToken)
    {
        // Récupérer la demande d'accès
        var accessRequest = await _accessRequestRepository.GetByIdAsync(request.AccessRequestId)
            ?? throw new NotFoundException(nameof(AccessRequest), request.AccessRequestId);

        // Vérifier que l'utilisateur actuel peut approuver
        if (!_currentUserService.IsInRole("Admin") && !_currentUserService.IsInRole("DO"))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        // Récupérer l'utilisateur qui approuve
        ///var email = _currentUserService.IsAppAuthentification ? request.ApproverEmail  : _currentUserService.Email ;

        // Récupérer l'utilisateur qui rejette
        var email = _currentUserService.Email;

        if (_currentUserService.IsAppAuthentification && !string.IsNullOrEmpty(request.ApproverEmail))
        {
            email = request.ApproverEmail;
        }

        var currentUser = (await _userRepository.GetByEmailAsync(email))
            ?? throw new NotFoundException($"ERR.General.UserNotExist {email}");

        // Approuver la demande (cela déclenchera l'événement AccessRequestApprovedEvent)
        accessRequest.Approve(currentUser.Id, request.Comments, _currentUserService.UserId, request.IsFromApplication);

        await _accessRequestRepository.ExecuteInTransactionAsync(async () =>
        {
            try
            {
                // Sauvegarder
                await _accessRequestRepository.UpdateAsync(accessRequest);

                // Créer l'utilisateur dans la BD
                var user = User.CreateExternalUser(
                    accessRequest.Email,
                    accessRequest.FirstName,
                    accessRequest.LastName,
                    null, // OrganizationName n'existe plus
                    null,//entraIdObjectId: sera mis à jour plus tard
                    _currentUserService.UserId);

                await _userRepository.AddAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du compte invité pour {Email}", accessRequest.Email);
            }

        });

        // Logger l'audit
        await _auditService.LogAsync(
            nameof(AccessRequest),
            accessRequest.Id,
            "Approve",
            null,
            newValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Status = accessRequest.Status.ToString(),
                ProcessedById = currentUser.Id,
                ProcessingComments = request.Comments,
                ProcessedDate = accessRequest.ProcessedDate
            }),
            cancellationToken);

        var dto = _mapper.Map<AccessRequestDto>(accessRequest);

        return new ApproveAccessRequestResponse
        {
            AccessRequest = dto,
            Message = "MSG.AccessRequest.Approved"
        };
    }

}