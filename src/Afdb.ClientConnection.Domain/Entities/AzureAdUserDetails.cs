using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class AzureAdUserDetails
{
    public string Id { get; init; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? JobTitle { get; init; }
    public string? Department { get; init; }

    public string? UserType { get; set; } = string.Empty;


    public AzureAdUserDetails(AzureAdUserDetailsLoadParam loadParam )
    {
        loadParam = loadParam ?? throw new ArgumentNullException(nameof(loadParam));
        
        Id = loadParam.Id;
        FirstName = loadParam.FirstName;
        LastName = loadParam.LastName;
        DisplayName = loadParam.DisplayName;
        Email = loadParam.Email;
        JobTitle = loadParam.JobTitle;
        Department = loadParam.Department;
        UserType = loadParam.UserType;
    }
}
