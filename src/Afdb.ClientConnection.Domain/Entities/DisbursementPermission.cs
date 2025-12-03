using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementPermission : BaseEntity
{
    public Guid BusinessProfileId { get; private set; }
    public Guid FunctionId { get; private set; }
    public bool CanConsult { get; private set; }
    public bool CanSubmit { get; private set; }

    public BusinessProfile BusinessProfile { get; private set; } = null!;
    public Function Function { get; private set; } = null!;

    private DisbursementPermission()
    {
    }

    public DisbursementPermission(DisbursementPermissionNewParam newParam )
    {
        BusinessProfileId = newParam.BusinessProfileId;
        FunctionId = newParam.FunctionId;
        CanConsult = newParam.CanConsult;
        CanSubmit = newParam.CanSubmit;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = "System";
    }

    public DisbursementPermission(DisbursementPermissionLoadParam loadParam)
    {
        Id = loadParam.Id;
        BusinessProfileId = loadParam.BusinessProfileId;
        FunctionId = loadParam.FunctionId;
        CanConsult = loadParam.CanConsult;
        CanSubmit = loadParam.CanSubmit;
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }

    public void Update(bool canConsult, bool canSubmit, string updatedBy)
    {
        CanConsult = canConsult;
        CanSubmit = canSubmit;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
