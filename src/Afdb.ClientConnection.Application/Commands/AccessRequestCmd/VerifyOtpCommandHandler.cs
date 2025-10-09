using Afdb.ClientConnection.Application.Common.Interfaces;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class VerifyOtpCommandHandler(IOtpService otpService) : IRequestHandler<VerifyOtpCommand, bool>
{
    private readonly IOtpService _otpService = otpService;

    public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        return await _otpService.IsOtpValidAsync(request.Email, request.Code);
    }
}
