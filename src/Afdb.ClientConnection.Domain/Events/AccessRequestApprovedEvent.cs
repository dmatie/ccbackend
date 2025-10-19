using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class AccessRequestApprovedEvent : DomainEvent
{
    public Guid AccessRequestId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public AccessRequestApprovedEvent(Guid accessRequestId, string email, string firstName,
        string lastName)
    {
        AccessRequestId = accessRequestId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}
