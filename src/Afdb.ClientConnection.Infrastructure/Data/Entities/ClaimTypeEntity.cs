using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("ClaimTypes")]
public class ClaimTypeEntity : BaseEntityConfiguration
{
    public string Name { get; set; } = default!;
    public string NameFr { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<ClaimEntity> Claims { get; set; } = default!;
}
