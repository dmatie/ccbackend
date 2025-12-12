using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class AccessRequestCreatedEvent : DomainEvent
{
    public Guid AccessRequestId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string RegistrationCode { get; }
    public string? Function { get; }
    public string? FunctionFr { get; }
    public string? BusinessProfile { get; }
    public string? Country { get; }
    public string? FinancingType { get; }
    public string Status { get; }
    public string[] ApproversEmail { get; }
    public SelectedProjectCreatedEvent[] SelectedProjects { get; }
    public List<string> Projects { get; }
    public string DocumentFileName { get; }


    public AccessRequestCreatedEvent(AccessRequestEventParams eventParams)
    {
        AccessRequestId = eventParams.AccessRequestId;
        Email = eventParams.Email;
        FirstName = eventParams.FirstName;
        LastName = eventParams.LastName;
        Function = eventParams.Function;
        FunctionFr = eventParams.FunctionFr;
        BusinessProfile = eventParams.BusinessProfile;
        Country = eventParams.Country;
        FinancingType = eventParams.FinancingType;
        Status = eventParams.Status;
        ApproversEmail = eventParams.ApproversEmail;
        RegistrationCode = eventParams.RegistrationCode;
        SelectedProjects = eventParams.Projects;
        DocumentFileName = eventParams.DocumentFileName;
        Projects = eventParams.Projects
            .Select(p => $"{p.SapCode} - {p.ProjectTitle}")
            .ToList();
    }
}


