using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Infrastructure.Services;

public sealed class NotificationService : INotificationService
{
    private readonly IPowerAutomateService _powerAutomateService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IPowerAutomateService powerAutomateService,
        ILogger<NotificationService> logger)
    {
        _powerAutomateService = powerAutomateService;
        _logger = logger;
    }

    public async Task SendNotificationAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Sending notification: EventType={EventType}, Recipient={Recipient}",
                request.EventType,
                request.Recipient);

            var payload = new
            {
                EventType = request.EventType.ToString(),
                Recipient = request.Recipient,
                RecipientName = request.RecipientName,
                AdditionalRecipients = request.AdditionalRecipients,
                CcRecipients = request.CcRecipients,
                Language = request.Language,
                Data = request.Data,
                Timestamp = DateTime.UtcNow
            };

            await _powerAutomateService.TriggerNotificationFlowAsync(
                payload,
                cancellationToken);

            _logger.LogInformation(
                "Notification sent successfully: EventType={EventType}, Recipient={Recipient}",
                request.EventType,
                request.Recipient);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send notification: EventType={EventType}, Recipient={Recipient}",
                request.EventType,
                request.Recipient);
        }
    }
}
