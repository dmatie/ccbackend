using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementSubmittedEventHandler : INotificationHandler<DisbursementSubmittedEvent>
{
    private readonly ILogger<DisbursementSubmittedEventHandler> _logger;

    public DisbursementSubmittedEventHandler(ILogger<DisbursementSubmittedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DisbursementSubmittedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling DisbursementSubmittedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}",
                notification.DisbursementId,
                notification.RequestNumber);

            _logger.LogInformation(
                "Successfully handled DisbursementSubmittedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling DisbursementSubmittedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            throw;
        }
    }
}
