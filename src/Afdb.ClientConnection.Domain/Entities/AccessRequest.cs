using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Domain.Events;
using System.Diagnostics.Metrics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class AccessRequest : AggregateRoot
{

    private readonly List<AccessRequestProject> _projects = [];

    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime RequestedDate { get; private set; }
    public DateTime? ProcessedDate { get; private set; }
    public Guid? ProcessedById { get; private set; }
    public string? ProcessingComments { get; private set; }
    public string? EntraIdObjectId { get; private set; } // Set after guest account creation

    // Nouvelles propriétés de liaison
    public Guid? FunctionId { get; private set; }
    public Guid? CountryId { get; private set; }
    public Guid? BusinessProfileId { get; private set; }
    public Guid? FinancingTypeId { get; private set; }


    public string[] ApproversEmail { get; private set; }

    // Navigation properties
    public User? ProcessedBy { get; private set; }
    public Function? Function { get; private set; }
    public Country? Country { get; private set; }
    public BusinessProfile? BusinessProfile { get; private set; }
    public FinancingType? FinancingType { get; private set; }

    public ICollection<AccessRequestProject> Projects => _projects;

    private AccessRequest() { } // For EF Core

    public AccessRequest( AccessRequestNewParam newParam )
    {
        if (string.IsNullOrWhiteSpace(newParam.Email))
            throw new ArgumentException("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.FirstName))
            throw new ArgumentException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.LastName))
            throw new ArgumentException("Last name cannot be empty");

        Email = newParam.Email.ToLowerInvariant();
        FirstName = newParam.FirstName;
        LastName = newParam.LastName;
        FunctionId = newParam.FunctionId;
        CountryId = newParam.CountryId;
        BusinessProfileId = newParam.BusinessProfileId;
        FinancingTypeId = newParam.FinancingTypeId;
        Function = newParam.Function;
        Country = newParam.Country;
        BusinessProfile = newParam.BusinessProfile;
        FinancingType = newParam.FinancingType;
        Status = RequestStatus.Pending;
        RequestedDate = DateTime.UtcNow;
        CreatedBy = newParam.CreatedBy;
        ApproversEmail = newParam.ApproversEmail ?? [];
        _projects = newParam.Projects;

        // Add domain event for Teams approval process with actual entity names
        AddDomainEvent(new AccessRequestCreatedEvent(Id, Email, FirstName, LastName,
             Function?.Name, BusinessProfile?.Name, Country?.Name, FinancingType?.Name, Status.ToString(), ApproversEmail));
    }

    public AccessRequest(AccessRequestLoadParam loadParam)
    {
        if (string.IsNullOrWhiteSpace(loadParam.Email))
            throw new ArgumentException("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.FirstName))
            throw new ArgumentException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.LastName))
            throw new ArgumentException("Last name cannot be empty");

        Id = loadParam.Id;
        Email = loadParam.Email.ToLowerInvariant();
        FirstName = loadParam.FirstName;
        LastName = loadParam.LastName;
        FunctionId = loadParam.FunctionId;
        CountryId = loadParam.CountryId;
        BusinessProfileId = loadParam.BusinessProfileId;
        FinancingTypeId = loadParam.FinancingTypeId;
        Function = loadParam.Function;
        Country = loadParam.Country;
        BusinessProfile = loadParam.BusinessProfile;
        FinancingType = loadParam.FinancingType;
        Status = RequestStatus.Pending;
        RequestedDate = DateTime.UtcNow;
        CreatedBy = loadParam.CreatedBy;
        ApproversEmail = loadParam.ApproversEmail ?? [];
        _projects = loadParam.Projects;
    }

    public void Approve(Guid processedById, string? comments, string updatedBy, bool isFromApplication)
    {
        if (Status != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved");

        Status = RequestStatus.Approved;
        ProcessedDate = DateTime.UtcNow;
        ProcessedById = processedById;
        ProcessingComments = comments;
        SetUpdated(updatedBy);

        // Add domain event for guest account creation
        if(isFromApplication)
            AddDomainEvent(new AccessRequestApprovedEvent(Id, Email, FirstName, LastName));
    }

    public void Reject(Guid processedById, string rejectionReason, string updatedBy, bool isFromApplication)
    {
        if (Status != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be rejected");

        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new ArgumentException("Rejection reason is required", nameof(rejectionReason));

        Status = RequestStatus.Rejected;
        ProcessedDate = DateTime.UtcNow;
        ProcessedById = processedById;
        ProcessingComments = rejectionReason;
        SetUpdated(updatedBy);

        // Add domain event for rejection notification
        if(isFromApplication)
            AddDomainEvent(new AccessRequestRejectedEvent(Id, Email, FirstName, LastName, rejectionReason));
    }

    public void SetEntraIdObjectId(string entraIdObjectId, string updatedBy)
    {
        if (Status != RequestStatus.Approved)
            throw new InvalidOperationException("Can only set Entra ID for approved requests");

        if (string.IsNullOrWhiteSpace(entraIdObjectId))
            throw new ArgumentException("Entra ID Object ID cannot be empty", nameof(entraIdObjectId));

        EntraIdObjectId = entraIdObjectId;
        SetUpdated(updatedBy);
    }

    public void UpdateFunction(Guid? functionId, Function? function, string updatedBy)
    {
        FunctionId = functionId;
        Function = function;
        SetUpdated(updatedBy);
    }

    public void UpdateCountry(Guid? countryId, Country? country, string updatedBy)
    {
        CountryId = countryId;
        Country = country;
        SetUpdated(updatedBy);
    }

    public void UpdateBusinessProfile(Guid? businessProfileId, BusinessProfile? businessProfile, string updatedBy)
    {
        BusinessProfileId = businessProfileId;
        BusinessProfile = businessProfile;
        SetUpdated(updatedBy);
    }

    public void UpdateFinancingType(Guid? financingTypeId, FinancingType? financingType, string updatedBy)
    {
        FinancingTypeId = financingTypeId;
        FinancingType = financingType;
        SetUpdated(updatedBy);
    }

    public void Update(AccessRequestNewParam updateParam )
    {
        if (!string.IsNullOrWhiteSpace(updateParam.FirstName))
            FirstName = updateParam.FirstName;
        if (!string.IsNullOrWhiteSpace(updateParam.LastName))
            LastName = updateParam.LastName;
        if (!string.IsNullOrWhiteSpace(updateParam.Email))
            Email = updateParam.Email.ToLowerInvariant();

        FunctionId = updateParam.FunctionId;
        CountryId = updateParam.CountryId;
        BusinessProfileId = updateParam.BusinessProfileId;
        FinancingTypeId = updateParam.FinancingTypeId;
        Function = updateParam.Function;
        Country = updateParam.Country;
        BusinessProfile = updateParam.BusinessProfile;
        FinancingType = updateParam.FinancingType;
        ApproversEmail = updateParam.ApproversEmail ?? ApproversEmail;
        Status = RequestStatus.Pending; // Reset status to Pending on update
        Projects.Clear();
        _projects.AddRange(updateParam.Projects);
        SetUpdated("System");
        // Add domain event for Teams approval process with actual entity names
        AddDomainEvent(new AccessRequestCreatedEvent(Id, Email, FirstName, LastName,
             Function?.Name, BusinessProfile?.Name, Country?.Name, FinancingType?.Name, Status.ToString(), ApproversEmail));

    }

    public string FullName => $"{FirstName} {LastName}";
    public bool CanBeProcessed => Status == RequestStatus.Pending;
    public bool IsProcessed => Status != RequestStatus.Pending;
    public bool HasEntraIdAccount => !string.IsNullOrWhiteSpace(EntraIdObjectId);
}