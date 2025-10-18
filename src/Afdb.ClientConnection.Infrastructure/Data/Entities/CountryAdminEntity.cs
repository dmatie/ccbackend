using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("CountryAdmins")]
public class CountryAdminEntity : BaseEntityConfiguration
{
    [Required]
    public Guid CountryId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = default!;
    public CountryEntity Country { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}
