using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Events;
using MediatR;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class AccessRequestApprovedEventHandler : INotificationHandler<AccessRequestApprovedEvent>
{
    private readonly IServiceBusService _serviceBusService;

    public AccessRequestApprovedEventHandler(IServiceBusService serviceBusService)
    {
        _serviceBusService = serviceBusService;
    }

    public async Task Handle(AccessRequestApprovedEvent notification, CancellationToken cancellationToken)
    {
        // Envoyer message au Service Bus pour déclencher Power Automate (création compte + email)
        await _serviceBusService.SendAccessRequestApprovedAsync(
            notification.AccessRequestId,
            notification.Email,
            notification.FirstName,
            notification.LastName,
            cancellationToken);
    }
}