using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementB1")]
public class DisbursementB1Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string GuaranteeDetails { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string IssuingBankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string IssuingBankAddress { get; set; } = default!;

    [Required]
    public Guid CurrencyId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string BeneficiaryName { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string BeneficiaryBPNumber { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string BeneficiaryAFDBContract { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string BeneficiaryAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryCity { get; set; } = default!;

    [Required]
    public Guid BeneficiaryCountryId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string GoodDescription { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ExecutingAgencyName { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ExecutingAgencyContactPerson { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string ExecutingAgencyAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ExecutingAgencyCity { get; set; } = default!;

    [Required]
    public Guid ExecutingAgencyCountryId { get; set; }

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string ExecutingAgencyEmail { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string ExecutingAgencyPhone { get; set; } = default!;

    public DisbursementEntity Disbursement { get; set; } = default!;
    public CurrencyEntity Currency { get; set; } = default!;
    public CountryEntity BeneficiaryCountry { get; set; } = default!;
    public CountryEntity ExecutingAgencyCountry { get; set; } = default!;
}
