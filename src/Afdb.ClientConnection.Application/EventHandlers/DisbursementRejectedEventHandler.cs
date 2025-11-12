using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementRejectedEventHandler : INotificationHandler<DisbursementRejectedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<DisbursementRejectedEventHandler> _logger;

    public DisbursementRejectedEventHandler(
        INotificationService notificationService,
        ILogger<DisbursementRejectedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(DisbursementRejectedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling DisbursementRejectedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}, Comment: {Comment}",
            notification.DisbursementId,
            notification.RequestNumber,
            notification.RejectionComment);

        var disbursementData = new Dictionary<string, object>
        {
            ["disbursementId"] = notification.DisbursementId,
            ["requestNumber"] = notification.RequestNumber,
            ["sapCodeProject"] = notification.SapCodeProject,
            ["loanGrantNumber"] = notification.LoanGrantNumber,
            ["disbursementTypeCode"] = notification.DisbursementTypeCode,
            ["disbursementTypeName"] = notification.DisbursementTypeName,
            ["rejectionComment"] = notification.RejectionComment,
            ["createdByFirstName"] = notification.CreatedByFirstName,
            ["createdByLastName"] = notification.CreatedByLastName,
            ["createdByEmail"] = notification.CreatedByEmail,
            ["rejectedByFirstName"] = notification.RejectedByFirstName,
            ["rejectedByLastName"] = notification.RejectedByLastName,
            ["rejectedByEmail"] = notification.RejectedByEmail,
            ["rejectedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["rejectedTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.DisbursementRejected,
                Recipient = notification.CreatedByEmail,
                RecipientName = $"{notification.CreatedByFirstName} {notification.CreatedByLastName}",
                Language = "",
                Data = NotificationRequest.ConvertDictionaryToArray(disbursementData)
            },
            cancellationToken);

        _logger.LogInformation(
            "Successfully notified disbursement creator about rejection: DisbursementId={DisbursementId}, Recipient={CreatedByEmail}",
            notification.DisbursementId,
            notification.CreatedByEmail);
    }
}
