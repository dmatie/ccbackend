using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementType : BaseEntity
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string NameFr { get; private set; }
    public string? Description { get; private set; }

    private DisbursementType() { }

    public DisbursementType(string code, string name, string nameFr, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(nameFr))
            throw new ArgumentException("NameFr cannot be empty");

        Code = code.ToUpper();
        Name = name;
        NameFr = nameFr;
        Description = description;
        CreatedBy = "System";
        CreatedAt = DateTime.UtcNow;
    }

    public DisbursementType(Guid id, string code, string name, string nameFr, string? description, DateTime createdAt, string createdBy, DateTime? updatedAt = null, string? updatedBy = null)
    {
        Id = id;
        Code = code;
        Name = name;
        NameFr = nameFr;
        Description = description;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
    }
}
