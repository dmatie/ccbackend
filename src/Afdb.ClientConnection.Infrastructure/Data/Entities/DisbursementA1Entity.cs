using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementA1")]
public class DisbursementA1Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(50)]
    public string BusinessPartnerNumber { get; set; } = default!;

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
    [MaxLength(200)]
    public string BeneficiaryCity { get; set; } = default!;

    [Required]
    public Guid BeneficiaryCountryId { get; set; }

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string BeneficiaryEmail { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BankName { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string BankAddress { get; set; } = default!;

    [Required]
    public Guid BankCountryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string BankAccountNumber { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string BankSwiftCode { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string SignatoryName { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string SignatoryContactPerson { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string SignatoryEmail { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string SignatoryPhone { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string SignatoryAddress { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string SignatoryCity { get; set; } = default!;

    [Required]
    public Guid SignatoryCountryId { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
    public CountryEntity BeneficiaryCountry { get; set; } = default!;
    public CountryEntity BankCountry { get; set; } = default!;
    public CountryEntity SignatoryCountry { get; set; } = default!;
}
