using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed class CreateOtpCommandHandler(IOtpService otpService,
    IAccessRequestRepository accessRequestRepository,
     INotificationService notificationService,
    IPowerAutomateService powerAutomateService) : IRequestHandler<CreateOtpCommand>
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

        var otpData = new Dictionary<string, object>
        {
            ["email"] = request.Email,
            ["otpCode"] = optCodeValue,
            ["expiresInMinutes"] = 10,
            ["createdDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["createdTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.OtpCreated,
                Recipient = request.Email,
                RecipientName = request.Email,
                Language = "",
                Data = NotificationRequest.ConvertDictionaryToArray(otpData)
            },
            cancellationToken);

        //await powerAutomateService.NotifyOtpCreatedAsync(request.Email, optCodeValue);
    }
}
