using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementRejectedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string RejectionComment { get; }
    public string CreatedByFirstName { get; }
    public string CreatedByLastName { get; }
    public string CreatedByEmail { get; }
    public string RejectedByFirstName { get; }
    public string RejectedByLastName { get; }
    public string RejectedByEmail { get; }
    public string DisbursementTypeCode { get; }
    public string DisbursementTypeName { get; }

    public DisbursementRejectedEvent(
        Guid disbursementId,
        string requestNumber,
        string sapCodeProject,
        string loanGrantNumber,
        string rejectionComment,
        User createdByUser,
        User rejectedByUser,
        DisbursementType disbursementType)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        RejectionComment = rejectionComment;
        CreatedByFirstName = createdByUser.FirstName;
        CreatedByLastName = createdByUser.LastName;
        CreatedByEmail = createdByUser.Email;
        RejectedByFirstName = rejectedByUser.FirstName;
        RejectedByLastName = rejectedByUser.LastName;
        RejectedByEmail = rejectedByUser.Email;
        DisbursementTypeCode = disbursementType.Code;
        DisbursementTypeName = disbursementType.Name;
    }
}
