namespace Afdb.ClientConnection.Infrastructure.Data.Entities;

public sealed class DisbursementPermissionEntity
{
    public Guid Id { get; set; }
    public Guid BusinessProfileId { get; set; }
    public Guid FunctionId { get; set; }
    public bool CanConsult { get; set; }
    public bool CanSubmit { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }

    public BusinessProfileEntity BusinessProfile { get; set; } = null!;
    public FunctionEntity Function { get; set; } = null!;
}
