using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Afdb.ClientConnection.Infrastructure.Services;

public sealed class NotificationService : INotificationService
{
    private readonly IPowerAutomateService _powerAutomateService;
    private readonly ILogger<NotificationService> _logger;
    private readonly FrontEndUrlsSettings _frontEndUrl;

    public NotificationService(
        IPowerAutomateService powerAutomateService,
        IOptions<FrontEndUrlsSettings> frontEndUrl,
        ILogger<NotificationService> logger)
    {
        _powerAutomateService = powerAutomateService;
        _logger = logger;
        _frontEndUrl = frontEndUrl.Value;
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

            var appUrlData = new Dictionary<string, object>
            {
                ["appurl"] = _frontEndUrl.BaseUrl,
                ["userreqsturl"] = _frontEndUrl.UserAccessRequest,
                ["userclaimurl"] = _frontEndUrl.UserClaim,
                ["adminclaimurl"] = _frontEndUrl.AdminClaim,
                ["userdisburl"] = _frontEndUrl.UserDisbursement,
                ["admindisburl"] = _frontEndUrl.AdminDisbursement
            };

            NotificationDataItem[] requestData = [..request.Data, .. NotificationRequest.ConvertDictionaryToArray(appUrlData)];

            var payload = new
            {
                EventType = request.EventType.ToString(),
                Recipient = request.Recipient,
                RecipientName = request.RecipientName,
                AdditionalRecipients = request.AdditionalRecipients,
                CcRecipients = request.CcRecipients,
                Language = request.Language,
                Data = requestData,
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
