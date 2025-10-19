using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementB1")]
public class DisbursementB1Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(500)]
    public string GuaranteePurpose { get; set; } = default!;

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
    [Column(TypeName = "decimal(18,2)")]
    public decimal GuaranteeAmount { get; set; }

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = default!;

    [Required]
    public DateTime ValidityStartDate { get; set; }

    [Required]
    public DateTime ValidityEndDate { get; set; }

    [Required]
    [MaxLength(2000)]
    public string GuaranteeTermsAndConditions { get; set; } = default!;

    [MaxLength(1000)]
    public string? SpecialInstructions { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
}
