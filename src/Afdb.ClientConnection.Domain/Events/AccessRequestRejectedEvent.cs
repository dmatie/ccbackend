using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class AccessRequestRejectedEvent : DomainEvent
{
    public Guid AccessRequestId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string RejectionReason { get; }
    public string Organization { get; }
    public string Country { get; }
    public string RejectedByEmail { get; }

    public AccessRequestRejectedEvent(AccessRequestRejectedEventNewParam newParam)
    {
        AccessRequestId = newParam.AccessRequestId;
        Email = newParam.Email;
        FirstName = newParam.FirstName;
        LastName = newParam.LastName;
        RejectionReason = newParam.RejectionReason;
        Organization = newParam.Organization;
        Country = newParam.Country;
        RejectedByEmail = newParam.RejectedByEmail;
    }
}


public sealed record AccessRequestRejectedEventNewParam
{
    public Guid AccessRequestId { get; init; }
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string RejectionReason { get; init; } = default!;
    public string Organization { get; init; } = default!;
    public string Country { get; init; } = default!;
    public string RejectedByEmail { get; init; } = default!;

}
