using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class AccessRequestSubmitedEvent : DomainEvent
{
    public Guid AccessRequestId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string RegistrationCode { get; }
    public string? Function { get; }
    public string? BusinessProfile { get; }
    public string? Country { get; }
    public string? FinancingType { get; }
    public string Status { get; }
    public string[] ApproversEmail { get; }
    public SelectedProjectCreatedEvent[] Projects { get; }

    public AccessRequestSubmitedEvent(AccessRequestCreatedEvent eventData)
    {

        AccessRequestId = eventData.AccessRequestId;
        Email = eventData.Email;
        FirstName = eventData.FirstName;
        LastName = eventData.LastName;
        Function = eventData.Function;
        BusinessProfile = eventData.BusinessProfile;
        Country = eventData.Country;
        FinancingType = eventData.FinancingType;
        Status = eventData.Status;
        ApproversEmail = eventData.ApproversEmail;
        RegistrationCode = eventData.RegistrationCode;
        Projects = eventData.Projects;
    }
}