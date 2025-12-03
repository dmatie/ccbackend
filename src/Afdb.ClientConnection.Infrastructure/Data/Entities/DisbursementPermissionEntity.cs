namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public sealed class DisbursementPermissionEntity : BaseEntityConfiguration
{
    public Guid BusinessProfileId { get; set; }
    public Guid FunctionId { get; set; }
    public bool CanConsult { get; set; }
    public bool CanSubmit { get; set; }
    public BusinessProfileEntity BusinessProfile { get; set; } = null!;
    public FunctionEntity Function { get; set; } = null!;
}
