using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementReSubmittedEventHandler : INotificationHandler<DisbursementReSubmittedEvent>
{
    private readonly ILogger<DisbursementReSubmittedEventHandler> _logger;

    public DisbursementReSubmittedEventHandler(ILogger<DisbursementReSubmittedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DisbursementReSubmittedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling DisbursementReSubmittedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}, Comment: {Comment}",
                notification.DisbursementId,
                notification.RequestNumber,
                notification.Comment);

            _logger.LogInformation(
                "Successfully handled DisbursementReSubmittedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling DisbursementReSubmittedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            throw;
        }
    }
}
