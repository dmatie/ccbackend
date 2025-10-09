using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class AccessRequestRejectedEvent : DomainEvent
{
    public Guid AccessRequestId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string RejectionReason { get; }

    public AccessRequestRejectedEvent(Guid accessRequestId, string email, string firstName,
        string lastName, string rejectionReason)
    {
        AccessRequestId = accessRequestId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RejectionReason = rejectionReason;
    }
}
