using Afdb.ClientConnection.Application.Common.Enums;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class ClaimResponseAddedEventHandler : INotificationHandler<ClaimProcessAddedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<ClaimResponseAddedEventHandler> _logger;

    public ClaimResponseAddedEventHandler(
        INotificationService notificationService,
        ILogger<ClaimResponseAddedEventHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Handle(ClaimProcessAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ClaimProcessAddedEvent for ClaimId: {ClaimId}, ProcessAuthor: {ProcessAuthorFirstName} {ProcessAuthorLastName}, Status: {ProcessStatus}",
            notification.ClaimId,
            notification.ProcessAuthorFirstName,
            notification.ProcessAuthorLastName,
            notification.ProcessStatus);

        var responseData = new Dictionary<string, object>
        {
            ["claimId"] = notification.ClaimId,
            ["claimTypeEn"] = notification.ClaimTypeEn,
            ["claimTypeFr"] = notification.ClaimTypeFr,
            ["country"] = notification.Country,
            ["claimComment"] = notification.Comment,
            ["authorFirstName"] = notification.AuthorFirstName,
            ["authorLastName"] = notification.AuthorLastName,
            ["authorEmail"] = notification.AuthorEmail,
            ["processComment"] = notification.ProcessComment,
            ["processAuthorFirstName"] = notification.ProcessAuthorFirstName,
            ["processAuthorLastName"] = notification.ProcessAuthorLastName,
            ["processStatus"] = notification.ProcessStatus,
            ["responseDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ["responseTime"] = DateTime.UtcNow.ToString("HH:mm")
        };

        await _notificationService.SendNotificationAsync(
            new NotificationRequest
            {
                EventType = NotificationEventType.ClaimResponseAdded,
                Recipient = notification.AuthorEmail,
                RecipientName = $"{notification.AuthorFirstName} {notification.AuthorLastName}",
                Language = "fr",
                Data = responseData
            },
            cancellationToken);

        _logger.LogInformation(
            "Successfully notified claim author about response: ClaimId={ClaimId}, Recipient={AuthorEmail}",
            notification.ClaimId,
            notification.AuthorEmail);
    }
}
