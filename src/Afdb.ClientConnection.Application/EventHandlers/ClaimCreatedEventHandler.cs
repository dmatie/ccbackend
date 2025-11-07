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

        await SendNotificationsToAssignedUsersAsync(notification, claimData, cancellationToken);

        await SendNotificationsToCcUsersAsync(notification, claimData, cancellationToken);

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

    private async Task SendNotificationsToAssignedUsersAsync(
        ClaimCreatedEvent notification,
        Dictionary<string, object> claimData,
        CancellationToken cancellationToken)
    {
        if (notification.AssignToEmail == null || notification.AssignToEmail.Length == 0)
        {
            _logger.LogInformation("No assigned users to notify for ClaimId: {ClaimId}", notification.ClaimId);
            return;
        }

        foreach (var assignedEmail in notification.AssignToEmail)
        {
            await _notificationService.SendNotificationAsync(
                new NotificationRequest
                {
                    EventType = NotificationEventType.ClaimCreated,
                    Recipient = assignedEmail,
                    RecipientName = assignedEmail,
                    Language = "en",
                    Data = claimData
                },
                cancellationToken);

            _logger.LogInformation(
                "Notification sent to assigned user: {AssignedEmail}",
                assignedEmail);
        }
    }

    private async Task SendNotificationsToCcUsersAsync(
        ClaimCreatedEvent notification,
        Dictionary<string, object> claimData,
        CancellationToken cancellationToken)
    {
        if (notification.AssignCcEmail == null || notification.AssignCcEmail.Length == 0)
        {
            _logger.LogInformation("No CC users to notify for ClaimId: {ClaimId}", notification.ClaimId);
            return;
        }

        foreach (var ccEmail in notification.AssignCcEmail)
        {
            await _notificationService.SendNotificationAsync(
                new NotificationRequest
                {
                    EventType = NotificationEventType.ClaimCreated,
                    Recipient = ccEmail,
                    RecipientName = ccEmail,
                    Language = "en",
                    Data = claimData
                },
                cancellationToken);

            _logger.LogInformation(
                "Notification sent to CC user: {CcEmail}",
                ccEmail);
        }
    }
}
