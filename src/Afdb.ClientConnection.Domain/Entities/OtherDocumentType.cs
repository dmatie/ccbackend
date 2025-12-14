using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class OtherDocumentType : AggregateRoot
{
    public string Name { get; private set; }
    public string NameFr { get; private set; }
    public bool IsActive { get; private set; }

    private OtherDocumentType() { }

    public OtherDocumentType(OtherDocumentTypeLoadParam loadParam)
    {
        Id = loadParam.Id;
        Name = loadParam.Name;
        NameFr = loadParam.NameFr;
        IsActive = loadParam.IsActive;
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }

    public OtherDocumentType(Guid id, string name, string nameFr, string createdBy = "System")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(nameFr))
            throw new ArgumentException("NameFr cannot be empty", nameof(nameFr));

        Id = id;
        Name = name;
        NameFr = nameFr;
        IsActive = true;
        CreatedBy = createdBy;
    }

    public void Update(string name, string nameFr, string updatedBy = "System")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(nameFr))
            throw new ArgumentException("NameFr cannot be empty", nameof(nameFr));

        Name = name;
        NameFr = nameFr;
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
