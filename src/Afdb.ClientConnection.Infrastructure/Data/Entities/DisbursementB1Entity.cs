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
    public string GuaranteeDetails { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string ConfirmingBank { get; set; } = default!;


    [Required]
    [MaxLength(200)]
    public string IssuingBankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string IssuingBankAdress { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GuaranteeAmount { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }


    [Required]
    [MaxLength(200)]
    public string BeneficiaryName { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryBPNumber { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryAFDBContract { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string BeneficiaryBankAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryCity { get; set; } = default!;

    [Required]
    public Guid BeneficiaryCountryId { get; set; }

    [Required]
    [MaxLength(500)]
    public string GoodDescription { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryLcContractRef { get; set; } = default!;


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
    public string ExecutingAgencyEmail { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ExecutingAgencyPhone { get; set; } = default!;


    public DisbursementEntity Disbursement { get; set; } = default!;

    public CountryEntity BeneficiaryCountry { get; set; } = default!;

    public CountryEntity ExecutingAgencyCountry { get; set; } = default!;
}
