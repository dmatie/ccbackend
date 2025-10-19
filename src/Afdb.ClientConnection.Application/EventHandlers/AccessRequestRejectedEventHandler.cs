using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Events;
using MediatR;

namespace Afdb.ClientConnection.Application.EventHandlers;

public class AccessRequestRejectedEventHandler : INotificationHandler<AccessRequestRejectedEvent>
{
    private readonly IServiceBusService _serviceBusService;

    public AccessRequestRejectedEventHandler(IServiceBusService serviceBusService)
    {
        _serviceBusService = serviceBusService;
    }

    public async Task Handle(AccessRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        // Envoyer message au Service Bus pour déclencher Power Automate (email de rejet)
        await _serviceBusService.SendAccessRequestRejectedAsync(
            notification.AccessRequestId,
            notification.Email,
            notification.FirstName,
            notification.LastName,
            notification.RejectionReason,
            cancellationToken);
    }
}