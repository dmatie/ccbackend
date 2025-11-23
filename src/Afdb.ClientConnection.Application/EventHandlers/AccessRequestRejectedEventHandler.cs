using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public class AccessRequestRejectedEventHandler : INotificationHandler<AccessRequestRejectedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<AccessRequestRejectedEventHandler> _logger;


    public AccessRequestRejectedEventHandler(INotificationService notificationService, 
        ILogger<AccessRequestRejectedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(AccessRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        // Envoyer message au Service Bus pour déclencher Power Automate (email de rejet)
        await SendMailNotificationForAppRejection(notification, cancellationToken);
    }

    private async Task SendMailNotificationForAppRejection(
        AccessRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        var rejectionData = new Dictionary<string, object>
        {
            ["requestId"] = notification.AccessRequestId,
            ["email"] = notification.Email,
            ["rejectionReason"] = notification.RejectionReason,
            ["firstName"] = notification.FirstName,
            ["lastName"] = notification.LastName,
            ["occurredAt"] = notification.OccurredAt,
            ["rejectedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["rejectedTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await _notificationService.SendNotificationAsync(
                    new NotificationRequest
                    {
                        EventType = NotificationEventType.AccessRequestRejected,
                        Recipient = notification.Email,
                        RecipientName = $"{notification.FirstName} {notification.LastName}",
                        Language = "",
                        Data = NotificationRequest.ConvertDictionaryToArray(rejectionData)
                    },
                    cancellationToken);

        _logger.LogInformation(
            "Successfully notified rejection  about AccessRequest: AccessRequestId={AccessRequestId}, Recipient={Email}",
            notification.AccessRequestId,
            notification.Email);
    }
}