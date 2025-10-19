using MediatR;

namespace Afdb.ClientConnection.Domain.Common;

public abstract class DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
