using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class SubmitOtherDocumentCommandHandler(
    IOtherDocumentRepository otherDocumentRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<SubmitOtherDocumentCommand, SubmitOtherDocumentResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository = otherDocumentRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<SubmitOtherDocumentResponse> Handle(
        SubmitOtherDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var otherDocument = await _otherDocumentRepository.GetByIdAsync(request.OtherDocumentId);
        if (otherDocument == null)
        {
            throw new NotFoundException("ERR.OtherDocument.NotFound");
        }

        if (otherDocument.Files.Count == 0)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Files", "ERR.OtherDocument.NoFilesAttached")
            });
        }

        otherDocument.Submit(_currentUserService.Email);

        await _otherDocumentRepository.UpdateAsync(otherDocument, cancellationToken);

        return new SubmitOtherDocumentResponse
        {
            Message = "MSG.OtherDocument.SubmittedSuccess"
        };
    }
}
