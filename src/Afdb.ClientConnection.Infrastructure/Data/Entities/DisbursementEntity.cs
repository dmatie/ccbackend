using Afdb.ClientConnection.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("Disbursements")]
public class DisbursementEntity : BaseEntityConfiguration
{
    [Required]
    [MaxLength(50)]
    public string RequestNumber { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string SapCodeProject { get; set; } = default!;

    [Required]
    [MaxLength(13)]
    public string LoanGrantNumber { get; set; } = default!;

    [Required]
    public Guid DisbursementTypeId { get; set; }

    [Required]
    public DisbursementStatus Status { get; set; }

    [Required]
    public Guid CreatedByUserId { get; set; }

    public DateTime? SubmittedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessedByUserId { get; set; }

    public DisbursementTypeEntity DisbursementType { get; set; } = default!;
    public UserEntity CreatedByUser { get; set; } = default!;
    public UserEntity? ProcessedByUser { get; set; }

    public ICollection<DisbursementProcessEntity> Processes { get; set; } = new List<DisbursementProcessEntity>();
    public ICollection<DisbursementDocumentEntity> Documents { get; set; } = new List<DisbursementDocumentEntity>();
    public DisbursementA1Entity? DisbursementA1 { get; set; }
    public DisbursementA2Entity? DisbursementA2 { get; set; }
    public DisbursementA3Entity? DisbursementA3 { get; set; }
    public DisbursementB1Entity? DisbursementB1 { get; set; }
}
