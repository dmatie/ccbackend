using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementCreatedEventHandler : INotificationHandler<DisbursementCreatedEvent>
{
    private readonly ILogger<DisbursementCreatedEventHandler> _logger;

    public DisbursementCreatedEventHandler(ILogger<DisbursementCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DisbursementCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling DisbursementCreatedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}",
                notification.DisbursementId,
                notification.RequestNumber);

            _logger.LogInformation(
                "Successfully handled DisbursementCreatedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling DisbursementCreatedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            throw;
        }
    }
}
