using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class ClaimCreatedEventHandler : INotificationHandler<ClaimCreatedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<ClaimCreatedEventHandler> _logger;

    public ClaimCreatedEventHandler(
        INotificationService notificationService,
        ILogger<ClaimCreatedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(ClaimCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ClaimCreatedEvent for ClaimId: {ClaimId}, Author: {AuthorEmail}",
            notification.ClaimId,
            notification.AuthorEmail);

        var claimData = new Dictionary<string, object>
        {
            ["claimId"] = notification.ClaimId,
            ["claimTypeEn"] = notification.ClaimTypeEn,
            ["claimTypeFr"] = notification.ClaimTypeFr,
            ["country"] = notification.Country,
            ["comment"] = notification.Comment,
            ["authorFirstName"] = notification.AuthorFirstName,
            ["authorLastName"] = notification.AuthorLastName,
            ["authorEmail"] = notification.AuthorEmail,
            ["createdDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["createdTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await SendNotificationToAuthorAsync(notification, claimData, cancellationToken);

        await SendNotificationToAssignedAndCcUsersAsync(notification, claimData, cancellationToken);

        _logger.LogInformation(
            "Successfully processed all notifications for ClaimId: {ClaimId}",
            notification.ClaimId);
    }

    private async Task SendNotificationToAuthorAsync(
        ClaimCreatedEvent notification,
        Dictionary<string, object> claimData,
        CancellationToken cancellationToken)
    {
        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.ClaimCreated,
                Recipient = notification.AuthorEmail,
                RecipientName = $"{notification.AuthorFirstName} {notification.AuthorLastName}",
                Language = "fr",
                Data = claimData
            },
            cancellationToken);

        _logger.LogInformation(
            "Notification sent to claim author: {AuthorEmail}",
            notification.AuthorEmail);
    }

    private async Task SendNotificationToAssignedAndCcUsersAsync(
        ClaimCreatedEvent notification,
        Dictionary<string, object> claimData,
        CancellationToken cancellationToken)
    {
        if ((notification.AssignToEmail == null || notification.AssignToEmail.Length == 0) &&
            (notification.AssignCcEmail == null || notification.AssignCcEmail.Length == 0))
        {
            _logger.LogInformation(
                "No assigned or CC users to notify for ClaimId: {ClaimId}",
                notification.ClaimId);
            return;
        }

        var assignToList = notification.AssignToEmail ?? Array.Empty<string>();
        var ccList = notification.AssignCcEmail ?? Array.Empty<string>();

        if (assignToList.Length == 0 && ccList.Length > 0)
        {
            _logger.LogWarning(
                "No assigned users but CC users exist for ClaimId: {ClaimId}. Using first CC as recipient.",
                notification.ClaimId);

            await _notificationService.SendNotificationAsync(
                new NotificationRequest
                {
                    EventType = NotificationEventType.ClaimCreated,
                    Recipient = ccList[0],
                    RecipientName = ccList[0],
                    AdditionalRecipients = null,
                    CcRecipients = ccList.Length > 1 ? ccList.Skip(1).ToArray() : null,
                    Language = "en",
                    Data = claimData
                },
                cancellationToken);

            _logger.LogInformation(
                "Notification sent to CC users (first as recipient): {CcCount} recipients",
                ccList.Length);
            return;
        }

        var primaryRecipient = assignToList[0];
        var additionalRecipients = assignToList.Length > 1 ? assignToList.Skip(1).ToArray() : null;

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.ClaimCreated,
                Recipient = primaryRecipient,
                RecipientName = primaryRecipient,
                AdditionalRecipients = additionalRecipients,
                CcRecipients = ccList.Length > 0 ? ccList : null,
                Language = "en",
                Data = claimData
            },
            cancellationToken);

        _logger.LogInformation(
            "Notification sent to assigned users: {AssignedCount} To, {CcCount} CC",
            assignToList.Length,
            ccList.Length);
    }
}
