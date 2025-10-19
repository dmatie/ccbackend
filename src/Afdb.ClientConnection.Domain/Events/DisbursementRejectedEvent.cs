using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementRejectedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string RejectionComment { get; }

    public DisbursementRejectedEvent(Guid disbursementId, string requestNumber, string sapCodeProject, string loanGrantNumber, string rejectionComment)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        RejectionComment = rejectionComment;
    }
}
