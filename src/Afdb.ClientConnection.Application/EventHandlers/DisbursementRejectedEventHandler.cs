using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementRejectedEventHandler : INotificationHandler<DisbursementRejectedEvent>
{
    private readonly ILogger<DisbursementRejectedEventHandler> _logger;

    public DisbursementRejectedEventHandler(ILogger<DisbursementRejectedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DisbursementRejectedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling DisbursementRejectedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}, Comment: {Comment}",
                notification.DisbursementId,
                notification.RequestNumber,
                notification.RejectionComment);

            _logger.LogInformation(
                "Successfully handled DisbursementRejectedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling DisbursementRejectedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            throw;
        }
    }
}
