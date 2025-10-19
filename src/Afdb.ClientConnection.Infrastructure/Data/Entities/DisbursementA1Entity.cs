using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementA1")]
public class DisbursementA1Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(500)]
    public string PaymentPurpose { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string BeneficiaryAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryBankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string BeneficiaryBankAddress { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string BeneficiaryAccountNumber { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string BeneficiarySwiftCode { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = default!;

    [MaxLength(200)]
    public string? IntermediaryBankName { get; set; }

    [MaxLength(50)]
    public string? IntermediaryBankSwiftCode { get; set; }

    [MaxLength(1000)]
    public string? SpecialInstructions { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
}
