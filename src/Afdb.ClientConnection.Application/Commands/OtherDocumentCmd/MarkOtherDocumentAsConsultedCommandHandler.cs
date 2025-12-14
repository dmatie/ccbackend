using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class MarkOtherDocumentAsConsultedCommandHandler(
    IOtherDocumentRepository otherDocumentRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<MarkOtherDocumentAsConsultedCommand, MarkOtherDocumentAsConsultedResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository = otherDocumentRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<MarkOtherDocumentAsConsultedResponse> Handle(
        MarkOtherDocumentAsConsultedCommand request,
        CancellationToken cancellationToken)
    {
        var otherDocument = await _otherDocumentRepository.GetByIdAsync(request.OtherDocumentId);
        if (otherDocument == null)
        {
            throw new NotFoundException("ERR.OtherDocument.NotFound");
        }

        otherDocument.MarkAsConsulted(_currentUserService.Email);

        await _otherDocumentRepository.UpdateAsync(otherDocument, cancellationToken);

        return new MarkOtherDocumentAsConsultedResponse
        {
            Message = "MSG.OtherDocument.MarkedAsConsultedSuccess"
        };
    }
}
