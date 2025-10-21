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
    public string BeneficiaryBpNumber { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryName { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BeneficiaryContactPerson { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string BeneficiaryAddress { get; set; } = default!;

    [Required]
    public Guid BeneficiaryCountryId { get; set; }

    [Required]
    [MaxLength(200)]
    public string BeneficiaryEmail { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string CorrespondentBankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string CorrespondentBankAddress { get; set; } = default!;

    [Required]
    public Guid CorrespondentBankCountryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CorrespondantAccountNumber { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string CorrespondentBankSwiftCode { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(200)]
    public string SignatoryName { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string SignatoryContactPerson { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string SignatoryAddress { get; set; } = default!;

    [Required]
    public Guid SignatoryCountryId { get; set; }

    [Required]
    [MaxLength(200)]
    public string SignatoryEmail { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string SignatoryPhone { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string SignatoryTitle { get; set; } = default!;

    public DisbursementEntity Disbursement { get; set; } = default!;
    public CountryEntity BeneficiaryCountry { get; set; }= default!;
    public CountryEntity CorrespondentBankCountry { get; set; } = default!;
    public CountryEntity SignatoryCountry { get; set; } = default!;
}
