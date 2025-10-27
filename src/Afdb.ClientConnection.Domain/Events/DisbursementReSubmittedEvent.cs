using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementReSubmittedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string Comment { get; }

    public DisbursementReSubmittedEvent(
        Guid disbursementId,
        string requestNumber,
        string sapCodeProject,
        string loanGrantNumber,
        string comment)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        Comment = comment;
    }
}
