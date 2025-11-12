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

        if ((notification.AssignToEmail == null || notification.AssignToEmail.Length == 0) &&
            (notification.AssignCcEmail == null || notification.AssignCcEmail.Length == 0))
        {
            _logger.LogInformation(
                "No assigned or CC users to notify for Disbursement: {DisbursementId}",
                notification.DisbursementId);
            return;
        }

        await SendNotificationToAuthorAsync(notification, disbursementData, cancellationToken);

        await SendNotificationToAssignedAndCcUsersAsync(notification, disbursementData, cancellationToken);


        _logger.LogInformation(
            "Successfully notified disbursement creator about submission: DisbursementId={DisbursementId}, Recipient={CreatedByEmail}",
            notification.DisbursementId,
            notification.CreatedByEmail);
    }

    private async Task SendNotificationToAuthorAsync(
        DisbursementSubmittedEvent notification,
        Dictionary<string, object> disbursementData,
        CancellationToken cancellationToken)
    {

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.DisbursementSubmittedAuthor,
                Recipient = notification.CreatedByEmail,
                RecipientName = $"{notification.CreatedByFirstName} {notification.CreatedByLastName}",
                Language = "fr",
                Data = NotificationRequest.ConvertDictionaryToArray(disbursementData)
            },
            cancellationToken);
        _logger.LogInformation(
            "Notification sent to disbursement author: {CreatedByEmail}",
            notification.CreatedByEmail);
    }

    private async Task SendNotificationToAssignedAndCcUsersAsync(
        DisbursementSubmittedEvent notification,
        Dictionary<string, object> disbursementData,
        CancellationToken cancellationToken)
    {
        if ((notification.AssignToEmail == null || notification.AssignToEmail.Length == 0) &&
            (notification.AssignCcEmail == null || notification.AssignCcEmail.Length == 0))
        {
            _logger.LogInformation(
                "No assigned or CC users to notify for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            return;
        }

        var assignToList = notification.AssignToEmail ?? Array.Empty<string>();
        var ccList = notification.AssignCcEmail ?? Array.Empty<string>();

        var primaryRecipient = assignToList[0];
        var additionalRecipients = assignToList.Length > 1 ? assignToList.Skip(1).ToArray() : null;
        var ccRecipients = ccList.Length > 0 ? ccList : null;

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.DisbursementSubmitted,
                Recipient = primaryRecipient,
                RecipientName = primaryRecipient,
                AdditionalRecipients = additionalRecipients,
                CcRecipients = ccRecipients,
                Language = "",
                Data = NotificationRequest.ConvertDictionaryToArray(disbursementData)
            },
            cancellationToken);
    }

}
