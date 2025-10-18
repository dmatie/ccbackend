using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class CreateOtpCommandHandler(IOtpService otpService,
    IAccessRequestRepository accessRequestRepository, IPowerAutomateService powerAutomateService) : IRequestHandler<CreateOtpCommand>
{
    public async Task Handle(CreateOtpCommand request, CancellationToken cancellationToken)
    {
        if (request.IsEmailExist)
        {
            bool exist = await accessRequestRepository.ExistsEmailAsync(request.Email);
            if (!exist)
                throw new NotFoundException("ERR.AccessRequest.AlreadyExistRequest");
        }

        string optCodeValue = await otpService.GenerateOtpForEmailAsync(request.Email);
        await powerAutomateService.NotifyOtpCreatedAsync(request.Email, optCodeValue);
    }
}
