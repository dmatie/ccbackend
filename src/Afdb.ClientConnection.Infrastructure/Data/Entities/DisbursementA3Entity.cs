using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("DisbursementA3")]
public class DisbursementA3Entity : BaseEntityConfiguration
{
    [Required]
    public Guid DisbursementId { get; set; }

    [Required]
    [MaxLength(200)]
    public string PeriodForUtilization { get; set; } = default!;

    [Required]
    public int ItemNumber { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string GoodDescription { get; set; } = default!;

    [Required]
    public Guid GoodOriginCountryId { get; set; }

    [Required]
    public int GoodQuantity { get; set; } = default!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualBudget { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BankShare { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AdvanceRequested { get; set; }

    [Required]
    public DateTime DateOfApproval { get; set; }

    public DisbursementEntity Disbursement { get; set; } = default!;

    public CountryEntity GoodOrginCountry { get; set; } = default!;
}
