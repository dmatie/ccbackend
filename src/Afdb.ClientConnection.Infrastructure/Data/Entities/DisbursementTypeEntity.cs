using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementTypes")]
public class DisbursementTypeEntity : BaseEntityConfiguration
{
    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string NameFr { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<DisbursementEntity> Disbursements { get; set; } = new List<DisbursementEntity>();
}
