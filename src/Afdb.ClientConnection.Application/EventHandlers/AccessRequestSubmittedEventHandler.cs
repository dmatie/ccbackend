using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class AccessRequestSubmittedEventHandler(
    INotificationService notificationService,
    ILogger<AccessRequestSubmittedEventHandler> logger) : INotificationHandler<AccessRequestSubmitedEvent>
{
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILogger<AccessRequestSubmittedEventHandler> _logger = logger;

    public async Task Handle(AccessRequestSubmitedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var notificationRequest = new NotificationRequest
            {
                EventType = NotificationEventType.AccessRequestSubmitted,
                RecipientEmail = notification.ApproversEmail,
                Data = new Dictionary<string, string>
                {
                    { "AccessRequestId", notification.AccessRequestId.ToString() },
                    { "Email", notification.Email },
                    { "FirstName", notification.FirstName },
                    { "LastName", notification.LastName },
                    { "Function", notification.Function ?? "N/A" },
                    { "BusinessProfile", notification.BusinessProfile ?? "N/A" },
                    { "Country", notification.Country ?? "N/A" },
                    { "FinancingType", notification.FinancingType ?? "N/A" },
                    { "Status", notification.Status },
                    { "RegistrationCode", notification.RegistrationCode }
                }
            };

            await _notificationService.SendNotificationAsync(notificationRequest, cancellationToken);

            _logger.LogInformation(
                "Notification envoyée pour soumission de demande d'accès {AccessRequestId}",
                notification.AccessRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erreur lors de l'envoi de la notification pour la soumission de la demande d'accès {AccessRequestId}",
                notification.AccessRequestId);
        }
    }
}
