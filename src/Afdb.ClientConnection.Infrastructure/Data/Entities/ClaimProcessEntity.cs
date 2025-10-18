using Afdb.ClientConnection.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("ClaimProcesses")]
public class ClaimProcessEntity : BaseEntityConfiguration
{
    [Required]
    public Guid ClaimId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public ClaimStatus Status { get; set; }


    [Required]
    [MaxLength(2000)]
    public string Comment { get; set; } = default!;

    public ClaimEntity Claim { get; set; } = default!;
    public UserEntity User { get; set; } = default!;

}