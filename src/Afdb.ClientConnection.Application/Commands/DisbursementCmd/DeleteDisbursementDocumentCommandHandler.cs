using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class DeleteDisbursementDocumentCommandHandler(
    IDisbursementRepository disbursementRepository,
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    ISharePointGraphService sharePointService) : IRequestHandler<DeleteDisbursementDocumentCommand, DeleteDisbursementDocumentResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISharePointGraphService _sharePointService = sharePointService;

    public async Task<DeleteDisbursementDocumentResponse> Handle(DeleteDisbursementDocumentCommand request, CancellationToken cancellationToken)
    {
        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken);

        if (disbursement == null)
            throw new NotFoundException($"ERR.Disbursement.NotFound:{request.DisbursementId}");

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email) ??
            throw new NotFoundException("ERR.General.UserNotFound");

        if (disbursement.CreatedByUserId != user.Id)
            throw new ForbiddenAccessException("ERR.Disbursement.NotOwner");

        var document = disbursement.Documents.FirstOrDefault(d => d.Id == request.DocumentId);

        if (document == null)
            throw new NotFoundException($"ERR.Disbursement.DocumentNotFound:{request.DocumentId}");

        try
        {
            await _sharePointService.DeleteFileByUrlAsync(document.DocumentUrl);
        }
        catch (Exception)
        {
        }

        disbursement.RemoveDocument(request.DocumentId);

        await _disbursementRepository.UpdateAsync(disbursement, cancellationToken);

        return new DeleteDisbursementDocumentResponse
        {
            Message = "Document deleted successfully"
        };
    }
}
