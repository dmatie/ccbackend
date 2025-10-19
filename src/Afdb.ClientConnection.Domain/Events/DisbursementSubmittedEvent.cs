using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementSubmittedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }

    public DisbursementSubmittedEvent(Guid disbursementId, string requestNumber, string sapCodeProject, string loanGrantNumber)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
    }
}
