using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class ClaimResponseAddedEventHandler : INotificationHandler<ClaimProcessAddedEvent>
{
    private readonly IPowerAutomateService _powerAutomateService;
    private readonly ILogger<ClaimResponseAddedEventHandler> _logger;

    public ClaimResponseAddedEventHandler(
        IPowerAutomateService powerAutomateService,
        ILogger<ClaimResponseAddedEventHandler> logger)
    {
        _powerAutomateService = powerAutomateService;
        _logger = logger;
    }

    public async Task Handle(ClaimProcessAddedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling ClaimProcessAddedEvent for ClaimId: {ClaimId}, ProcessAuthor: {ProcessAuthorFirstName} {ProcessAuthorLastName}, Status: {ProcessStatus}",
                notification.ClaimId,
                notification.ProcessAuthorFirstName,
                notification.ProcessAuthorLastName,
                notification.ProcessStatus);

            await _powerAutomateService.NotifyClaimResponseAddedAsync(notification);

            _logger.LogInformation(
                "Successfully notified PowerAutomate for ClaimResponseAdded on ClaimId: {ClaimId}",
                notification.ClaimId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling ClaimProcessAddedEvent for ClaimId: {ClaimId}",
                notification.ClaimId);
            throw;
        }
    }
}
