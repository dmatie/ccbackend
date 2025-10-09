using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Country : BaseEntity
{
    public string Name { get; private set; }
    public string NameFr { get; private set; }
    public string Code { get; private set; }
    public bool IsActive { get; private set; }

    private Country() { } // For EF Core

    public Country(Guid id, string name, string nameFr, string code, string createdBy = "System")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(nameFr))
            throw new ArgumentException("NameFr cannot be empty", nameof(nameFr));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));

        Id= id;
        Name = name;
        NameFr = nameFr;
        Code = code;
        IsActive = true;
        CreatedBy = createdBy;
    }

    public void Update(string name, string nameFr, string code, string updatedBy = "System")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(nameFr))
            throw new ArgumentException("NameFr cannot be empty", nameof(nameFr));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));

        Name = name;
        NameFr = nameFr;
        Code = code;
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
