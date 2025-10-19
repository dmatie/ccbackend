using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementBackedToClientEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string Comment { get; }
    public Guid CreatedByUserId { get; }

    public DisbursementBackedToClientEvent(Guid disbursementId, string requestNumber, string sapCodeProject, string loanGrantNumber, string comment, Guid createdByUserId)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        Comment = comment;
        CreatedByUserId = createdByUserId;
    }
}
