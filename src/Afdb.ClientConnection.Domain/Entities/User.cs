using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class User : AggregateRoot
{
    private readonly List<CountryAdmin> _countries = [];

    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public string? EntraIdObjectId { get; private set; } // Azure AD Object ID
    public string? OrganizationName { get; private set; }

    // Navigation properties
    public ICollection<AccessRequest> AccessRequests { get; private set; } = new List<AccessRequest>();
    public ICollection<CountryAdmin> Countries => _countries;

    private User() { } // For EF Core

    // Constructor pour utilisateurs internes (déjà dans Entra ID)
    public User(string email, string firstName, string lastName, UserRole role,
        string entraIdObjectId, string createdBy, string? organizationName = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        Email = email.ToLowerInvariant();
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        IsActive = true;
        EntraIdObjectId = entraIdObjectId;
        OrganizationName = organizationName;
        CreatedBy = createdBy;
    }

    public User(UserNewParam newParam)
    {
        Email = newParam.Email;
        FirstName = newParam.FirstName;
        LastName = newParam.LastName;
        Role = newParam.Role;
        IsActive = true;
        EntraIdObjectId = newParam.EntraIdObjectId;
        OrganizationName = newParam.OrganizationName;
        CreatedBy = newParam.CreatedBy;
        CreatedAt = DateTime.UtcNow;
        _countries = newParam.Countries ?? [];
    }

    public User(UserLoadParam loadParam)
    {
        Id = loadParam.Id;
        Email = loadParam.Email;
        FirstName = loadParam.FirstName;
        LastName = loadParam.LastName;
        Role = loadParam.Role;
        IsActive = loadParam.IsActive;
        EntraIdObjectId = loadParam.EntraIdObjectId;
        OrganizationName = loadParam.OrganizationName;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = DateTime.UtcNow;
        _countries = loadParam.Countries ?? [];
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }


    // Constructor pour utilisateurs externes (après création du compte invité)
    public static User CreateExternalUser(string email, string firstName, string lastName,
        string? organizationName, string entraIdObjectId, string createdBy)
    {
        return new User(email, firstName, lastName, UserRole.ExternalUser,
            entraIdObjectId, createdBy, organizationName);
    }

    public void UpdateProfile(string firstName, string lastName, string? organizationName, string updatedBy)
    {
        FirstName = firstName;
        LastName = lastName;
        OrganizationName = organizationName;
        SetUpdated(updatedBy);
    }

    public void ChangeRole(UserRole newRole, string updatedBy)
    {
        if (Role == newRole) return;

        Role = newRole;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        IsActive = true;
        SetUpdated(updatedBy);
    }

    public void SetEntraIdObjectId(string entraIdObjectId, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(entraIdObjectId))
            throw new ArgumentException("Entra ID Object ID cannot be empty", nameof(entraIdObjectId));

        EntraIdObjectId = entraIdObjectId;
        SetUpdated(updatedBy);
    }

    public string FullName => $"{FirstName} {LastName}";
    public bool IsInternal => Role != UserRole.ExternalUser;
    public bool IsExternal => Role == UserRole.ExternalUser;
    public bool HasEntraIdAccount => !string.IsNullOrWhiteSpace(EntraIdObjectId);
    public bool RequiresCountryAssignment => Role == UserRole.DO || Role == UserRole.DA;
}