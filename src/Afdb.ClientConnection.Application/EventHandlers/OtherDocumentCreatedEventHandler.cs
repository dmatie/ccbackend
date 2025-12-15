using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class OtherDocumentCreatedEventHandler : INotificationHandler<OtherDocumentCreatedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<OtherDocumentCreatedEventHandler> _logger;

    public OtherDocumentCreatedEventHandler(
        INotificationService notificationService,
        ILogger<OtherDocumentCreatedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(OtherDocumentCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling OtherDocumentCreatedEvent for OtherDocumentId: {OtherDocumentId}, DocumentName: {DocumentName}",
            notification.OtherDocumentId,
            notification.DocumentName);

        var otherDocumentData = new Dictionary<string, object>
        {
            ["otherDocumentId"] = notification.OtherDocumentId,
            ["documentName"] = notification.DocumentName,
            ["year"] = notification.Year,
            ["sapCode"] = notification.SAPCode,
            ["loanNumber"] = notification.LoanNumber,
            ["otherDocumentTypeName"] = notification.OtherDocumentTypeName,
            ["otherDocumentTypeNameFr"] = notification.OtherDocumentTypeNameFr,
            ["createdByFirstName"] = notification.CreatedByFirstName,
            ["createdByLastName"] = notification.CreatedByLastName,
            ["createdByEmail"] = notification.CreatedByEmail,
            ["fileNames"] = string.Join(", ", notification.FileNames),
            ["createdDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["createdTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await SendNotificationToAuthorAsync(notification, otherDocumentData, cancellationToken);

        if ((notification.AssignToEmail != null && notification.AssignToEmail.Length > 0) ||
            (notification.AssignCcEmail != null && notification.AssignCcEmail.Length > 0))
        {
            await SendNotificationToInternalUsersAsync(notification, otherDocumentData, cancellationToken);
        }
        else
        {
            _logger.LogInformation(
                "No assigned or CC users to notify for OtherDocumentId: {OtherDocumentId}",
                notification.OtherDocumentId);
        }

        _logger.LogInformation(
            "Successfully handled OtherDocumentCreatedEvent for OtherDocumentId: {OtherDocumentId}",
            notification.OtherDocumentId);
    }

    private async Task SendNotificationToAuthorAsync(
        OtherDocumentCreatedEvent notification,
        Dictionary<string, object> otherDocumentData,
        CancellationToken cancellationToken)
    {
        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.OtherDocumentCreated,
                Recipient = notification.CreatedByEmail,
                RecipientName = $"{notification.CreatedByFirstName} {notification.CreatedByLastName}",
                Language = "fr",
                Data = NotificationRequest.ConvertDictionaryToArray(otherDocumentData)
            },
            cancellationToken);

        _logger.LogInformation(
            "Notification sent to document creator: {CreatedByEmail}",
            notification.CreatedByEmail);
    }

    private async Task SendNotificationToInternalUsersAsync(
        OtherDocumentCreatedEvent notification,
        Dictionary<string, object> otherDocumentData,
        CancellationToken cancellationToken)
    {
        var assignToList = notification.AssignToEmail ?? Array.Empty<string>();
        var ccList = notification.AssignCcEmail ?? Array.Empty<string>();

        var primaryRecipient = assignToList[0];
        var additionalRecipients = assignToList.Length > 1 ? assignToList.Skip(1).ToArray() : null;
        var ccRecipients = ccList.Length > 0 ? ccList : null;

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.OtherDocumentCreatedInternal,
                Recipient = primaryRecipient,
                RecipientName = primaryRecipient,
                AdditionalRecipients = additionalRecipients,
                CcRecipients = ccRecipients,
                Language = "",
                Data = NotificationRequest.ConvertDictionaryToArray(otherDocumentData)
            },
            cancellationToken);

        _logger.LogInformation(
            "Notification sent to internal users for OtherDocumentId: {OtherDocumentId}",
            notification.OtherDocumentId);
    }
}
