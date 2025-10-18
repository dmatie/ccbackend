using Afdb.ClientConnection.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("Claims")]
public class ClaimEntity : BaseEntityConfiguration
{
    [Required]
    public Guid ClaimTypeId { get; set; }

    [Required]
    public Guid CountryId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public ClaimStatus Status { get; set; }

    public DateTime? ClosedAt { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Comment { get; set; } = default!;

    public UserEntity User { get; set; } = default!;
    public CountryEntity Country { get; set; } = default!;
    public ClaimTypeEntity ClaimType { get; set; } = default!;
    public ICollection<ClaimProcessEntity> Processes { get; set; } = default!;
}