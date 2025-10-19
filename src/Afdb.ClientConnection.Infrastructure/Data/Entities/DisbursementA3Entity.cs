using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementA3")]
public class DisbursementA3Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(500)]
    public string AdvancePurpose { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string RecipientName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string RecipientAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string RecipientBankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string RecipientBankAddress { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string RecipientAccountNumber { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string RecipientSwiftCode { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = default!;

    [Required]
    public DateTime ExpectedUsageDate { get; set; }

    [Required]
    [MaxLength(1000)]
    public string JustificationForAdvance { get; set; } = default!;

    [MaxLength(1000)]
    public string? RepaymentTerms { get; set; }

    [MaxLength(1000)]
    public string? SpecialInstructions { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
}
