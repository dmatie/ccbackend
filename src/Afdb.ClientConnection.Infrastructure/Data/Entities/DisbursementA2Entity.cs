using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementA2")]
public class DisbursementA2Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(500)]
    public string ReimbursementPurpose { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ClaimantName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string ClaimantAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ClaimantBankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string ClaimantBankAddress { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string ClaimantAccountNumber { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string ClaimantSwiftCode { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = default!;

    [Required]
    public DateTime ExpenseDate { get; set; }

    [Required]
    [MaxLength(1000)]
    public string ExpenseDescription { get; set; } = default!;

    [MaxLength(1000)]
    public string? SupportingDocuments { get; set; }

    [MaxLength(1000)]
    public string? SpecialInstructions { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
}
