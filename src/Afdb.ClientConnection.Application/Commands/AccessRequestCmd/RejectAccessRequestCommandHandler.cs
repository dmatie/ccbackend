using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class RejectAccessRequestCommandHandler(
    IAccessRequestRepository accessRequestRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IAuditService auditService,
    IMapper mapper) : IRequestHandler<RejectAccessRequestCommand, RejectAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;

    public async Task<RejectAccessRequestResponse> Handle(RejectAccessRequestCommand request, CancellationToken cancellationToken)
    {
        // Récupérer la demande d'accès
        var accessRequest = await _accessRequestRepository.GetByIdAsync(request.AccessRequestId)
            ?? throw new NotFoundException(nameof(AccessRequest), request.AccessRequestId);

        // Vérifier que l'utilisateur actuel peut rejeter
        if (!_currentUserService.IsInRole("Admin") && !_currentUserService.IsInRole("DO"))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        // Récupérer l'utilisateur qui rejette
        var email =   _currentUserService.Email;

        if(_currentUserService.IsAppAuthentification && !string.IsNullOrEmpty(request.ApproverEmail))
        {
            email = request.ApproverEmail;
        }

        var currentUser = (await _userRepository.GetByEmailAsync(email))
           ?? throw new NotFoundException($"ERR.General.UserNotExist {email}");

        // Rejeter la demande (cela déclenchera l'événement AccessRequestRejectedEvent)
        accessRequest.Reject(currentUser.Id, request.RejectionReason, _currentUserService.UserId, request.IsFromApplication);

        // Sauvegarder
        await _accessRequestRepository.UpdateAsync(accessRequest);

        // Logger l'audit
        await _auditService.LogAsync(
            nameof(AccessRequest),
            accessRequest.Id,
            "Reject",
            null,
            newValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Status = accessRequest.Status.ToString(),
                ProcessedById = currentUser.Id,
                ProcessingComments = request.RejectionReason,
                ProcessedDate = accessRequest.ProcessedDate
            }),
            cancellationToken);

        var dto = _mapper.Map<AccessRequestDto>(accessRequest);

        return new RejectAccessRequestResponse
        {
            AccessRequest = dto,
            Message = "MSG.AccessRequest.Rejected"
        };
    }
}
