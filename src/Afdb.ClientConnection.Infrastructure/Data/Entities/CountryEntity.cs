using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("Countries")]
public class CountryEntity : BaseEntityConfiguration
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string NameFr { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public ICollection<ClaimEntity> Claims { get; set; } = default!;
    public ICollection<CountryAdminEntity> CountryAdmins { get; set; } = default!;
}
