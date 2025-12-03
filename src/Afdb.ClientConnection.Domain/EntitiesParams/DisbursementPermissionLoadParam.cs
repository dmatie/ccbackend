namespace Afdb.ClientConnection.Domain.EntitiesParams;

public sealed record DisbursementPermissionLoadParam :  CommonLoadParam
{
    public Guid BusinessProfileId { get; init; }
    public Guid FunctionId { get; init; }
    public bool CanConsult { get; init; }
    public bool CanSubmit { get; init; }
}
