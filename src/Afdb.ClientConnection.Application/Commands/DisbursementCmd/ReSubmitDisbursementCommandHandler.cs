using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Helpers;
using Afdb.ClientConnection.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class ReSubmitDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IFileValidationService fileValidationService,
    IDisbursementDocumentService disbursementDocumentService,
    IMapper mapper) : IRequestHandler<ReSubmitDisbursementCommand, ReSubmitDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IDisbursementDocumentService _disbursementDocumentService = disbursementDocumentService;
    private readonly IMapper _mapper = mapper;

    public async Task<ReSubmitDisbursementResponse> Handle(ReSubmitDisbursementCommand request, CancellationToken cancellationToken)
    {
        if (request.AdditionalDocuments != null && request.AdditionalDocuments.Count > 0)
        {
            await _fileValidationService.ValidateAndThrowAsync(request.AdditionalDocuments, "AdditionalDocuments");
        }

        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        disbursement.Resubmit(user, request.Comment);

        if (request.AdditionalDocuments != null && request.AdditionalDocuments.Count > 0)
        {
            await _disbursementDocumentService.UploadAndAttachDocumentsAsync(
                disbursement,
                request.AdditionalDocuments,
                cancellationToken);
        }

        var updatedDisbursement = await _disbursementRepository.UpdateAsync(disbursement, cancellationToken);

        return new ReSubmitDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(updatedDisbursement),
            Message = "MSG.DISBURSEMENT.RESUBMITTED_SUCCESS"
        };
    }
}
