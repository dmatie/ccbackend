using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class ApproveAccessRequestByAppCommandHandler(
    IAccessRequestRepository accessRequestRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IAuditService auditService,
    IMapper mapper,
    IServiceBusService serviceBusService) : IRequestHandler<ApproveAccessRequestByAppCommand, ApproveAccessRequestByAppResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;
    private readonly IServiceBusService _serviceBusService = serviceBusService;
    

    public async Task<ApproveAccessRequestByAppResponse> Handle(ApproveAccessRequestByAppCommand request, CancellationToken cancellationToken)
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
        var email =  _currentUserService.Email ;

        var currentUser = (await _userRepository.GetByEmailAsync(email))
            ?? throw new NotFoundException($"ERR.General.UserNotExist {email}");

        // Approuver la demande (cela déclenchera l'événement AccessRequestApprovedEvent)
        accessRequest.Approve(currentUser.Id, request.Comments, _currentUserService.UserId, email, request.IsFromApplication);

        //Mettre à jour la demande d'accès
        await _accessRequestRepository.UpdateAsync(accessRequest);

        // Logger l'audit
        await _auditService.LogAsync(
            nameof(AccessRequest),
            accessRequest.Id,
            "Approve By App",
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

        return new ApproveAccessRequestByAppResponse
        {
            AccessRequest = dto,
            Message = "MSG.AccessRequest.Approved"
        };
    }
}