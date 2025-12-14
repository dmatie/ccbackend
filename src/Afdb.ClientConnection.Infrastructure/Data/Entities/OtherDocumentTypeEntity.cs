namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public class OtherDocumentTypeEntity : BaseEntityConfiguration
{
    public string Name { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public bool IsActive { get; set; } 

    public ICollection<OtherDocumentEntity> OtherDocuments { get; set; } = new List<OtherDocumentEntity>();
}
