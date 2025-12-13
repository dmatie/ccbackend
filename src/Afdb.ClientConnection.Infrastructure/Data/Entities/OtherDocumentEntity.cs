using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public class OtherDocumentEntity : BaseEntityConfiguration
{
    public Guid OtherDocumentTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public OtherDocumentStatus Status { get; set; }
    public string SAPCode { get; set; } = string.Empty;
    public string LoanNumber { get; set; } = string.Empty;

    public OtherDocumentTypeEntity? OtherDocumentType { get; set; }
    public UserEntity? User { get; set; }
    public ICollection<OtherDocumentFileEntity> Files { get; set; } = new List<OtherDocumentFileEntity>();
}
