using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Events;
using MediatR;

namespace Afdb.ClientConnection.Application.EventHandlers;

public sealed class AccessRequestCreatedEventHandler : INotificationHandler<AccessRequestCreatedEvent>
{
    private readonly IServiceBusService _serviceBusService;

    public AccessRequestCreatedEventHandler(IServiceBusService serviceBusService)
    {
        _serviceBusService = serviceBusService;
    }

    public async Task Handle(AccessRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Envoyer message au Service Bus pour déclencher le processus d'approbation Teams
        await _serviceBusService.SendAccessRequestCreatedAsync(
         notification.AccessRequestId,
         notification.Email,
         notification.FirstName,
         notification.LastName,
         notification.Function,
         notification.BusinessProfile,
         notification.Country,
         notification.FinancingType,
         notification.Status,
         notification.ApproversEmail,
         cancellationToken);
    }
}