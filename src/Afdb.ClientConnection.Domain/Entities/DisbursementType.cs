using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementType : BaseEntity
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private DisbursementType() { }

    public DisbursementType(string code, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty");

        Code = code.ToUpper();
        Name = name;
        Description = description;
        CreatedBy = "System";
    }
}
