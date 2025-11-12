using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementBackedToClientEventHandler : INotificationHandler<DisbursementBackedToClientEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<DisbursementBackedToClientEventHandler> _logger;

    public DisbursementBackedToClientEventHandler(
        INotificationService notificationService,
        ILogger<DisbursementBackedToClientEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(DisbursementBackedToClientEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling DisbursementBackedToClientEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}, Comment: {Comment}",
            notification.DisbursementId,
            notification.RequestNumber,
            notification.Comment);

        var disbursementData = new Dictionary<string, object>
        {
            ["disbursementId"] = notification.DisbursementId,
            ["requestNumber"] = notification.RequestNumber,
            ["sapCodeProject"] = notification.SapCodeProject,
            ["loanGrantNumber"] = notification.LoanGrantNumber,
            ["disbursementTypeCode"] = notification.DisbursementTypeCode,
            ["disbursementTypeName"] = notification.DisbursementTypeName,
            ["comment"] = notification.Comment,
            ["createdByFirstName"] = notification.CreatedByFirstName,
            ["createdByLastName"] = notification.CreatedByLastName,
            ["createdByEmail"] = notification.CreatedByEmail,
            ["processedByFirstName"] = notification.ProcessedByFirstName,
            ["processedByLastName"] = notification.ProcessedByLastName,
            ["processedByEmail"] = notification.ProcessedByEmail,
            ["backedDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["backedTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.DisbursementBackedToClient,
                Recipient = notification.CreatedByEmail,
                RecipientName = $"{notification.CreatedByFirstName} {notification.CreatedByLastName}",
                Language = "",
                Data = NotificationRequest.ConvertDictionaryToArray(disbursementData)
            },
            cancellationToken);

        _logger.LogInformation(
            "Successfully notified disbursement creator about backed to client: DisbursementId={DisbursementId}, Recipient={CreatedByEmail}",
            notification.DisbursementId,
            notification.CreatedByEmail);
    }
}
