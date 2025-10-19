using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class AccessRequestCreatedEvent : DomainEvent
{
    public Guid AccessRequestId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? Function { get; }
    public string? BusinessProfile { get; }
    public string? Country { get; }
    public string? FinancingType { get; }
    public string Status { get; }
    public string[] ApproversEmail { get; }

    public AccessRequestCreatedEvent(Guid accessRequestId, string email, string firstName,
        string lastName, string? function, string? businessProfile, string? country,
        string? financingType, string status, string[] approversEmail)
    {
        AccessRequestId = accessRequestId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Function = function;
        BusinessProfile = businessProfile;
        Country = country;
        FinancingType = financingType;
        Status = status;
        ApproversEmail = approversEmail;
    }
}