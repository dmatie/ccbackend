using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class ClaimCreatedEventHandler : INotificationHandler<ClaimCreatedEvent>
{
    private readonly IPowerAutomateService _powerAutomateService;
    private readonly ILogger<ClaimCreatedEventHandler> _logger;

    public ClaimCreatedEventHandler(
        IPowerAutomateService powerAutomateService,
        ILogger<ClaimCreatedEventHandler> logger)
    {
        _powerAutomateService = powerAutomateService;
        _logger = logger;
    }

    public async Task Handle(ClaimCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling ClaimCreatedEvent for ClaimId: {ClaimId}, Author: {AuthorEmail}",
                notification.ClaimId,
                notification.AuthorEmail);

            await _powerAutomateService.NotifyClaimCreatedAsync(notification);

            _logger.LogInformation(
                "Successfully notified PowerAutomate for ClaimId: {ClaimId}",
                notification.ClaimId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling ClaimCreatedEvent for ClaimId: {ClaimId}",
                notification.ClaimId);
            throw new InvalidOperationException("Error handling ClaimCreatedEvent for ClaimId: {ClaimId}");
        }
    }
}
