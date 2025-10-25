using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementProcess : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public DisbursementStatus Status { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public string Comment { get; private set; }
    public Disbursement? Disbursement { get; private set; }
    public User? CreatedByUser { get; private set; }

    private DisbursementProcess() { }

    public DisbursementProcess(DisbursementProcessNewParam newParam)
    {
        if (newParam.ProcessedByUserId == Guid.Empty)
            throw new ArgumentException("ProcessedByUserId must be a valid GUID");

        if (string.IsNullOrWhiteSpace(newParam.Comment))
            throw new ArgumentException("Comment cannot be empty");

        Status = newParam.Status;
        CreatedByUserId = newParam.ProcessedByUserId;
        CreatedByUser = newParam.ProcessedByUser;
        Comment = newParam.Comment;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementProcess(DisbursementProcessLoadParam loadParam)
    {
        Id = loadParam.Id;
        DisbursementId = loadParam.DisbursementId;
        Status = loadParam.Status;
        CreatedByUserId = loadParam.CreatedByUserId;
        CreatedByUser = loadParam.CreatedByUser;
        Comment = loadParam.Comment;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }
}
