using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementApprovedEventHandler : INotificationHandler<DisbursementApprovedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<DisbursementApprovedEventHandler> _logger;

    public DisbursementApprovedEventHandler(
        INotificationService notificationService,
        ILogger<DisbursementApprovedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(DisbursementApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling DisbursementApprovedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}",
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
            ["approvedByFirstName"] = notification.ApprovedByFirstName,
            ["approvedByLastName"] = notification.ApprovedByLastName,
            ["approvedByEmail"] = notification.ApprovedByEmail,
            ["approvedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["approvedTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.DisbursementApproved,
                Recipient = notification.CreatedByEmail,
                RecipientName = $"{notification.CreatedByFirstName} {notification.CreatedByLastName}",
                Language = "fr",
                Data = disbursementData
            },
            cancellationToken);

        _logger.LogInformation(
            "Successfully notified disbursement creator about approval: DisbursementId={DisbursementId}, Recipient={CreatedByEmail}",
            notification.DisbursementId,
            notification.CreatedByEmail);
    }
}
