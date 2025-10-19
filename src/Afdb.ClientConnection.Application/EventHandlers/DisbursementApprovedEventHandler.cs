using Afdb.ClientConnection.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class DisbursementApprovedEventHandler : INotificationHandler<DisbursementApprovedEvent>
{
    private readonly ILogger<DisbursementApprovedEventHandler> _logger;

    public DisbursementApprovedEventHandler(ILogger<DisbursementApprovedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DisbursementApprovedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Handling DisbursementApprovedEvent for DisbursementId: {DisbursementId}, RequestNumber: {RequestNumber}",
                notification.DisbursementId,
                notification.RequestNumber);

            _logger.LogInformation(
                "Successfully handled DisbursementApprovedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling DisbursementApprovedEvent for DisbursementId: {DisbursementId}",
                notification.DisbursementId);
            throw;
        }
    }
}
