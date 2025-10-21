using Afdb.ClientConnection.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("AccessRequests")]
public class AccessRequestEntity : BaseEntityConfiguration
{
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public RequestStatus Status { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public Guid? ProcessedById { get; set; }

    [MaxLength(1000)]
    public string? ProcessingComments { get; set; }

    [MaxLength(255)]
    public string? EntraIdObjectId { get; set; }

    // Nouvelles propriétés de liaison
    public Guid? FunctionEntityId { get; set; }
    public Guid? CountryEntityId { get; set; }
    public Guid? BusinessProfileEntityId { get; set; }
    public Guid? FinancingTypeEntityId { get; set; }


    // Navigation properties
    [ForeignKey(nameof(ProcessedById))]
    public UserEntity? ProcessedBy { get; set; }

    [ForeignKey(nameof(FunctionEntityId))]
    public FunctionEntity? Function { get; set; }

    [ForeignKey(nameof(CountryEntityId))]
    public CountryEntity? Country { get; set; }

    [ForeignKey(nameof(BusinessProfileEntityId))]
    public BusinessProfileEntity? BusinessProfile { get; set; }
    [ForeignKey(nameof(FinancingTypeEntityId))]
    public FinancingTypeEntity? FinancingType { get; set; }
    public ICollection<AccessRequestProjectEntity> Projects { get; set; } = default!;
}