using Afdb.ClientConnection.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementProcesses")]
public class DisbursementProcessEntity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    public DisbursementStatus Status { get; set; }

    [Required]
    public Guid CreatedByUserId { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }

    [MaxLength(1000)]
    public string? DocumentUrl { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
    public UserEntity CreatedByUser { get; set; } = default!;
}
