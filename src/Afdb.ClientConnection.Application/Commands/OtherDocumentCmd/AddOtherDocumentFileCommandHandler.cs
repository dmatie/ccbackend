using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class AddOtherDocumentFileCommandHandler(
    IOtherDocumentRepository otherDocumentRepository,
    IOtherDocumentService otherDocumentService,
    IFileValidationService fileValidationService)
    : IRequestHandler<AddOtherDocumentFileCommand, AddOtherDocumentFileResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository = otherDocumentRepository;
    private readonly IOtherDocumentService _otherDocumentService = otherDocumentService;
    private readonly IFileValidationService _fileValidationService = fileValidationService;

    public async Task<AddOtherDocumentFileResponse> Handle(
        AddOtherDocumentFileCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Files == null || request.Files.Count == 0)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Files", "ERR.OtherDocument.FilesRequired")
            });
        }

        await _fileValidationService.ValidateAndThrowAsync(request.Files, "Files");

        var otherDocument = await _otherDocumentRepository.GetByIdAsync(request.OtherDocumentId);
        if (otherDocument == null)
        {
            throw new NotFoundException("ERR.OtherDocument.NotFound");
        }

        await _otherDocumentService.UploadAndAttachFilesAsync(
            otherDocument,
            request.Files,
            cancellationToken);

        await _otherDocumentRepository.UpdateAsync(otherDocument, cancellationToken);

        return new AddOtherDocumentFileResponse
        {
            Message = "MSG.OtherDocument.FilesUploadedSuccess",
            FilesUploaded = request.Files.Count
        };
    }
}
