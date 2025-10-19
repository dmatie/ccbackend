using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed class GetAccessRequestQueryHandler(
    IAccessRequestRepository accessRequestRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetAccessRequestQuery, GetAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetAccessRequestResponse> Handle(GetAccessRequestQuery request, CancellationToken cancellationToken)
    {
        var accessRequest = await _accessRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(AccessRequest), request.Id);

        // Vérifier les permissions d'accès
        if (!CanAccessRequest(accessRequest))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        var dto = _mapper.Map<AccessRequestDto>(accessRequest);

        return new GetAccessRequestResponse
        {
            AccessRequest = dto
        };
    }

    private bool CanAccessRequest(AccessRequest accessRequest)
    {
        // Admin et DO peuvent voir toutes les demandes
        if (_currentUserService.IsInRole("Admin") || _currentUserService.IsInRole("DO"))
            return true;

        // L'utilisateur peut voir ses propres demandes (si authentifié)
        if (_currentUserService.IsAuthenticated &&
            accessRequest.CreatedBy == _currentUserService.UserId)
            return true;

        // Demandes publiques (non authentifiées) peuvent être consultées par leur créateur
        // via un token ou système de vérification (à implémenter selon besoins)

        return false;
    }
}