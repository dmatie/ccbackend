using Afdb.ClientConnection.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public abstract class BaseEntityConfiguration
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [Required]
    [MaxLength(255)]
    public string CreatedBy { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? UpdatedBy { get; set; }

    [NotMapped] // Ignorée par EF Core
    public List<DomainEvent> DomainEvents { get; set; } = new();

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }
}
