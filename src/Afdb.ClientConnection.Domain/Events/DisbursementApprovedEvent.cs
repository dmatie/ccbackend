using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementApprovedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string CreatedByFirstName { get; }
    public string CreatedByLastName { get; }
    public string CreatedByEmail { get; }
    public string ApprovedByFirstName { get; }
    public string ApprovedByLastName { get; }
    public string ApprovedByEmail { get; }
    public string DisbursementTypeCode { get; }
    public string DisbursementTypeName { get; }

    public DisbursementApprovedEvent(
        Guid disbursementId,
        string requestNumber,
        string sapCodeProject,
        string loanGrantNumber,
        User createdByUser,
        User approvedByUser,
        DisbursementType disbursementType)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        CreatedByFirstName = createdByUser.FirstName;
        CreatedByLastName = createdByUser.LastName;
        CreatedByEmail = createdByUser.Email;
        ApprovedByFirstName = approvedByUser.FirstName;
        ApprovedByLastName = approvedByUser.LastName;
        ApprovedByEmail = approvedByUser.Email;
        DisbursementTypeCode = disbursementType.Code;
        DisbursementTypeName = disbursementType.Name;
    }
}
