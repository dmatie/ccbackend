using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementTypes")]
public class DisbursementTypeEntity : BaseEntityConfiguration
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string NameFr { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string FormCode { get; set; } = default!;
}
