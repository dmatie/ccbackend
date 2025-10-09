using Afdb.ClientConnection.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("Users")]
public class UserEntity : BaseEntityConfiguration
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

    public UserRole Role { get; set; }

    public bool IsActive { get; set; }

    [MaxLength(255)]
    public string? EntraIdObjectId { get; set; }

    [MaxLength(200)]
    public string? OrganizationName { get; set; }

    // Navigation properties
    public ICollection<AccessRequestEntity> ProcessedAccessRequests { get; set; } = new List<AccessRequestEntity>();
    public ICollection<ClaimEntity> Claims { get; set; } = new List<ClaimEntity>();
    public ICollection<ClaimProcessEntity> ClaimProcesses { get; set; } = new List<ClaimProcessEntity>();
}