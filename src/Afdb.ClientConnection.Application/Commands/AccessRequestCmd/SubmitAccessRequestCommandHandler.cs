using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Helpers;
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
    IAccessRequestDocumentService documentService,
    IFileValidationService fileValidationService,
    IMapper mapper,
    ILogger<SubmitAccessRequestCommandHandler> logger) : IRequestHandler<SubmitAccessRequestCommand, SubmitAccessRequestResponse>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IAuditService _auditService = auditService;
    private readonly IAccessRequestDocumentService _documentService = documentService;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<SubmitAccessRequestCommandHandler> _logger = logger;

    public async Task<SubmitAccessRequestResponse> Handle(SubmitAccessRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Document == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Document","ERR.AccessRequest.DocumentRequired")
            });


        await _fileValidationService.ValidateAndThrowAsync(new[] { request.Document }, "Document");

        if (!request.Document.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Document","ERR.AccessRequest.OnlyPdfAllowed")
            });

        var accessRequest = await _accessRequestRepository.GetByIdAsync(request.AccessRequestId)
            ?? throw new NotFoundException(nameof(AccessRequest), request.AccessRequestId);

        await _documentService.UploadAndAttachDocumentAsync(
                accessRequest,
                request.Document,
                cancellationToken);


        accessRequest.Submit();

        await _accessRequestRepository.UpdateAsync(accessRequest);

        await _auditService.LogAsync(
            nameof(AccessRequest),
            accessRequest.Id,
            "Submit",
            oldValues: null,
            newValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Status = accessRequest.Status.ToString(),
                DocumentFileName = request.Document.FileName
            }),
            cancellationToken);

        var dto = _mapper.Map<AccessRequestDto>(accessRequest);

        return new SubmitAccessRequestResponse
        {
            AccessRequest = dto,
            Message = "MSG.AccessRequest.Submitted"
        };
    }
}
