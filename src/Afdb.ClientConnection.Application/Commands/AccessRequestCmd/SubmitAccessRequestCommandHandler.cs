using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class SubmitAccessRequestCommandHandler(
    IAccessRequestRepository accessRequestRepository,
    IAuditService auditService,
    IMapper mapper,
    ILogger<SubmitAccessRequestCommandHandler> logger) : IRequestHandler<SubmitAccessRequestCommand, SubmitAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IAuditService _auditService = auditService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<SubmitAccessRequestCommandHandler> _logger = logger;

    public async Task<SubmitAccessRequestResponse> Handle(SubmitAccessRequestCommand request, CancellationToken cancellationToken)
    {
        var accessRequest = await _accessRequestRepository.GetByIdAsync(request.AccessRequestId)
            ?? throw new NotFoundException(nameof(AccessRequest), request.AccessRequestId);

        try
        {
            accessRequest.Submit();

            await _accessRequestRepository.UpdateAsync(accessRequest);

            await _auditService.LogAsync(
                nameof(AccessRequest),
                accessRequest.Id,
                "Submit",
                oldValues: null,
                newValues: System.Text.Json.JsonSerializer.Serialize(new
                {
                    Status = accessRequest.Status.ToString()
                }),
                cancellationToken);

            var dto = _mapper.Map<AccessRequestDto>(accessRequest);

            return new SubmitAccessRequestResponse
            {
                AccessRequest = dto,
                Message = "MSG.AccessRequest.Submitted"
            };
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Tentative de soumission invalide pour la demande {AccessRequestId}", request.AccessRequestId);
            throw new ValidationException("ERR.AccessRequest.InvalidStatusForSubmit");
        }
    }
}
