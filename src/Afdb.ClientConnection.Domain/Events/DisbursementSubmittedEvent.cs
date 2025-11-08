using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementSubmittedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string CreatedByFirstName { get; }
    public string CreatedByLastName { get; }
    public string CreatedByEmail { get; }
    public string DisbursementTypeCode { get; }
    public string DisbursementTypeName { get; }

    public DisbursementSubmittedEvent(
        Guid disbursementId,
        string requestNumber,
        string sapCodeProject,
        string loanGrantNumber,
        User createdByUser,
        DisbursementType disbursementType)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        CreatedByFirstName = createdByUser.FirstName;
        CreatedByLastName = createdByUser.LastName;
        CreatedByEmail = createdByUser.Email;
        DisbursementTypeCode = disbursementType.Code;
        DisbursementTypeName = disbursementType.Name;
    }
}
