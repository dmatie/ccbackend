using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementSubmittedEventHandler : INotificationHandler<DisbursementSubmittedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<DisbursementSubmittedEventHandler> _logger;

    public DisbursementSubmittedEventHandler(
        INotificationService notificationService,
        ILogger<DisbursementSubmittedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(DisbursementSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling DisbursementSubmittedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}",
            notification.DisbursementId,
            notification.RequestNumber);

        var disbursementData = new Dictionary<string, object>
        {
            ["disbursementId"] = notification.DisbursementId,
            ["requestNumber"] = notification.RequestNumber,
            ["sapCodeProject"] = notification.SapCodeProject,
            ["loanGrantNumber"] = notification.LoanGrantNumber,
            ["disbursementTypeCode"] = notification.DisbursementTypeCode,
            ["disbursementTypeName"] = notification.DisbursementTypeName,
            ["createdByFirstName"] = notification.CreatedByFirstName,
            ["createdByLastName"] = notification.CreatedByLastName,
            ["createdByEmail"] = notification.CreatedByEmail,
            ["submittedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["submittedTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.DisbursementSubmitted,
                Recipient = notification.CreatedByEmail,
                RecipientName = $"{notification.CreatedByFirstName} {notification.CreatedByLastName}",
                Language = "fr",
                Data = disbursementData
            },
            cancellationToken);

        _logger.LogInformation(
            "Successfully notified disbursement creator about submission: DisbursementId={DisbursementId}, Recipient={CreatedByEmail}",
            notification.DisbursementId,
            notification.CreatedByEmail);
    }
}
