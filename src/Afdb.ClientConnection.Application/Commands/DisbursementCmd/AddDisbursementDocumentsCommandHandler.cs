using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class AddDisbursementDocumentsCommandHandler(
    IDisbursementRepository disbursementRepository,
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IFileValidationService fileValidationService,
    IDisbursementDocumentService disbursementDocumentService) : IRequestHandler<AddDisbursementDocumentsCommand, AddDisbursementDocumentsResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IDisbursementDocumentService _disbursementDocumentService = disbursementDocumentService;

    public async Task<AddDisbursementDocumentsResponse> Handle(AddDisbursementDocumentsCommand request, CancellationToken cancellationToken)
    {
        await _fileValidationService.ValidateAndThrowAsync(request.Documents, "Documents");

        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken);

        if (disbursement == null)
            throw new NotFoundException($"ERR.Disbursement.NotFound:{request.DisbursementId}");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email) ??
            throw new NotFoundException("ERR.General.UserNotFound");

        if (disbursement.CreatedByUserId != user.Id)
            throw new ForbiddenAccessException("ERR.Disbursement.NotOwner");

        var currentDocumentCount = disbursement.Documents.Count;
        var newDocumentCount = request.Documents.Count;
        var totalDocuments = currentDocumentCount + newDocumentCount;

        if (totalDocuments > 3)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Documents",
                    $"ERR.Disbursement.MaxDocumentsExceeded:Max 3 documents allowed. Current: {currentDocumentCount}, Trying to add: {newDocumentCount}")
            });

        await _disbursementDocumentService.UploadAndAttachDocumentsAsync(
            disbursement,
            request.Documents,
            cancellationToken);

        return new AddDisbursementDocumentsResponse
        {
            Message = "Documents added successfully",
            DocumentsAdded = newDocumentCount
        };
    }
}
