using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class AccessRequestSubmittedEventHandler : INotificationHandler<AccessRequestSubmitedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly IServiceBusService _serviceBusService;
    private readonly ILogger<AccessRequestSubmittedEventHandler> _logger;

    public AccessRequestSubmittedEventHandler(
        INotificationService notificationService,
        IServiceBusService serviceBusService,
        ILogger<AccessRequestSubmittedEventHandler> logger)
    {
        _notificationService = notificationService;
        _serviceBusService = serviceBusService;
        _logger = logger;
    }

    public async Task Handle(AccessRequestSubmitedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _serviceBusService.SendAccessRequestCreatedAsync(
                notification.AccessRequestId,
                notification.Email,
                notification.FirstName,
                notification.LastName,
                notification.Function,
                notification.BusinessProfile,
                notification.Country,
                notification.FinancingType,
                notification.Status,
                notification.ApproversEmail,
                notification.DocumentFileName,
                notification.RegistrationCode,
                cancellationToken);

            var data = new Dictionary<string, object>
            {
                ["code"] = notification.RegistrationCode,
                ["firstName"] = notification.FirstName,
                ["lastName"] = notification.LastName,
                ["email"] = notification.Email,
                ["function"] = notification.Function ?? "N/A"
            };

            var notificationRequest = new NotificationRequest
            {
                EventType = NotificationEventType.AccessRequestSubmitted,
                Recipient = notification.Email,
                RecipientName = $"{notification.FirstName} {notification.LastName}",
                Data = NotificationRequest.ConvertDictionaryToArray(data),
                Language = "en"
            };

            await _notificationService.SendNotificationAsync(notificationRequest, cancellationToken);

            _logger.LogInformation(
                "Notification and Service Bus message sent for access request submission {AccessRequestId} with document {DocumentFileName}",
                notification.AccessRequestId,
                notification.DocumentFileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erreur lors de l'envoi de la notification pour la soumission de la demande d'accès {AccessRequestId}",
                notification.AccessRequestId);
        }
    }
}