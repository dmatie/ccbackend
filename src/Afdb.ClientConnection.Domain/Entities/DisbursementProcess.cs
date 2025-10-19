using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class DisbursementProcess : BaseEntity
{
    public Guid DisbursementId { get; private set; }
    public DisbursementStatus Status { get; private set; }
    public Guid ProcessedByUserId { get; private set; }
    public string Comment { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    public Disbursement? Disbursement { get; private set; }
    public User? ProcessedByUser { get; private set; }

    private DisbursementProcess() { }

    public DisbursementProcess(DisbursementProcessNewParam newParam)
    {
        if (newParam.DisbursementId == Guid.Empty)
            throw new ArgumentException("DisbursementId must be a valid GUID");

        if (newParam.ProcessedByUserId == Guid.Empty)
            throw new ArgumentException("ProcessedByUserId must be a valid GUID");

        if (string.IsNullOrWhiteSpace(newParam.Comment))
            throw new ArgumentException("Comment cannot be empty");

        DisbursementId = newParam.DisbursementId;
        Status = newParam.Status;
        ProcessedByUserId = newParam.ProcessedByUserId;
        ProcessedByUser = newParam.ProcessedByUser;
        Comment = newParam.Comment;
        ProcessedAt = DateTime.UtcNow;
        CreatedBy = newParam.CreatedBy;
    }

    public DisbursementProcess(DisbursementProcessLoadParam loadParam)
    {
        Id = loadParam.Id;
        DisbursementId = loadParam.DisbursementId;
        Status = loadParam.Status;
        ProcessedByUserId = loadParam.ProcessedByUserId;
        ProcessedByUser = loadParam.ProcessedByUser;
        Comment = loadParam.Comment;
        ProcessedAt = loadParam.ProcessedAt;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
    }
}
