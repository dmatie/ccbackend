using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementA2")]
public class DisbursementA2Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = default!;

    [Required]
    public Guid CurrencyId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Contractor { get; set; } = default!;

    [Required]
    [MaxLength(1000)]
    public string GoodDescription { get; set; } = default!;

    [Required]
    public Guid GoodOriginCountryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ContractBorrowerReference { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string ContractAfDBReference { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ContractValue { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ContractBankShare { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ContractAmountPreviouslyPaid { get; set; }

    [Required]
    [MaxLength(100)]
    public string InvoiceRef { get; set; } = default!;

    [Required]
    public DateTime InvoiceDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal InvoiceAmount { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;
    public CurrencyEntity Currency { get; set; } = default!;
    public CountryEntity GoodOriginCountry { get; set; } = default!;
}
