using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

[Table("AccessRequestsProjects")]
public class AccessRequestProjectEntity : BaseEntityConfiguration
{
    public Guid AccessRequestId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SapCode { get; set; } = default!;
    public AccessRequestEntity AccessRequest { get; set; } = default!;
}
