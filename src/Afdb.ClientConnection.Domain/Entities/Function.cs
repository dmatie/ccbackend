using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Function : BaseEntity
{
    public string Name { get; private set; }
    public string NameFr { get; private set; }
    public string Code { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private Function() { } // For EF Core

    public Function(FunctionLoadParam loadParam)
    {
        if (string.IsNullOrWhiteSpace(loadParam.Name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.NameFr))
            throw new ArgumentException("NameFr cannot be empty");

        Id = loadParam.Id;
        Name = loadParam.Name;
        NameFr = loadParam.NameFr;
        Code = loadParam.Code;
        Description = loadParam.Description;
        IsActive = loadParam.IsActive;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }

    public void Update(string name, string? description = null, string updatedBy = "System")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        SetUpdated(updatedBy);
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
