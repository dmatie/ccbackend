using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class ClaimType : AggregateRoot
{
    public string Name { get; set; }
    public string NameFr { get; set; } 
    public string Description { get; set; }
    public List<Claim> Claims { get; set; } = default!;

    public ClaimType(ClaimTypeLoadParam loadParam)
    {
        Id= loadParam.Id;
        Name = loadParam.Name;
        NameFr = loadParam.NameFr;
        Description = loadParam.Description;
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }
}