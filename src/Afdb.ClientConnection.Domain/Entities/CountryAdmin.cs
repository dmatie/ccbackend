using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class CountryAdmin : BaseEntity
{
    public Guid CountryId { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsActive { get; private set; }
    public User User { get; private set; } = default!;
    public Country Country { get; private set; } = default!;

    private CountryAdmin() { } // For EF Core

    public CountryAdmin(CountryAdminNewParam newParam)
    {
        CountryId = newParam.CountryId;
        UserId = newParam.UserId;
        IsActive = newParam.IsActive;
    }
    public CountryAdmin(CountryAdminLoadParam loadParam)
    {
        Id = loadParam.Id;
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
        Country = loadParam.Country;
        User = loadParam.User;
        CountryId = loadParam.CountryId;
        UserId = loadParam.UserId;
        IsActive = loadParam.IsActive;
    }

    public void Deactivate(string updatedBy = "System")
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }

    public void Activate(string updatedBy = "System")
    {
        IsActive = true;
        SetUpdated(updatedBy);
    }
}
