using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("Currencies")]
public class CurrencyEntity : BaseEntityConfiguration
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = default!;

    [MaxLength(10)]
    public string? Symbol { get; set; }
}
