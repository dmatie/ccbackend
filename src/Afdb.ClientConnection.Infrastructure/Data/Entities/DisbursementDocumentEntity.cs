using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementDocuments")]
public class DisbursementDocumentEntity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(500)]
    public string FileName { get; set; } = default!;

    [Required]
    [MaxLength(1000)]
    public string DocumentUrl { get; set; } = default!;

    public DisbursementEntity Disbursement { get; set; } = default!;
}
