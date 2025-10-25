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
    public string Contractor { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string GoodDescription { get; set; } = default!;

    [Required]
    public Guid GoodOriginCountryId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ContractBorrowerReference { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ContractAfDBReference { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ContractValue { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string ContractBankShare { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ContractAmountPreviouslyPaid { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string InvoiceRef { get; set; } = default!;

    [Required]
    public DateTime InvoiceDate { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal InvoiceAmount { get; set; } = default!;

    [Required]
    public DateTime PaymentDateOfPayment { get; set; } = default!;


    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PaymentAmountWithdrawn { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string PaymentEvidenceOfPayment { get; set; } = default!;
   
    public DisbursementEntity Disbursement { get; set; } = default!;

    public CountryEntity GoodOrginCountry { get; set; } = default!;
}
