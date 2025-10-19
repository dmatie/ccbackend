using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementBackedToClientEventHandler : INotificationHandler<DisbursementBackedToClientEvent>
{
    private readonly ILogger<DisbursementBackedToClientEventHandler> _logger;

    public DisbursementBackedToClientEventHandler(ILogger<DisbursementBackedToClientEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DisbursementBackedToClientEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling DisbursementBackedToClientEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}, Comment: {Comment}",
                notification.DisbursementId,
                notification.RequestNumber,
                notification.Comment);

            _logger.LogInformation(
                "Successfully handled DisbursementBackedToClientEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling DisbursementBackedToClientEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            throw;
        }
    }
}
