using Afdb.ClientConnection.Domain.Common;

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

    public static DisbursementPermission Create(
        Guid businessProfileId,
        Guid functionId,
        bool canConsult,
        bool canSubmit)
    {
        return new DisbursementPermission
        {
            Id = Guid.NewGuid(),
            BusinessProfileId = businessProfileId,
            FunctionId = functionId,
            CanConsult = canConsult,
            CanSubmit = canSubmit,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = "System"
        };
    }

    public void Update(bool canConsult, bool canSubmit, string updatedBy)
    {
        CanConsult = canConsult;
        CanSubmit = canSubmit;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
