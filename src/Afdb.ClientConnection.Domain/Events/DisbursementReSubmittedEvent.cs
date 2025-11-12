using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementReSubmittedEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string Comment { get; }
    public string CreatedByFirstName { get; }
    public string CreatedByLastName { get; }
    public string CreatedByEmail { get; }
    public string DisbursementTypeCode { get; }
    public string DisbursementTypeName { get; }
    public string[] AssignToEmail { get; }
    public string[] AssignCcEmail { get; }

    public DisbursementReSubmittedEvent(
        Guid disbursementId,
        string requestNumber,
        string sapCodeProject,
        string loanGrantNumber,
        string comment,
        User createdByUser,
        DisbursementType disbursementType, string[] assignTo, string[] assignCC)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        Comment = comment;
        CreatedByFirstName = createdByUser.FirstName;
        CreatedByLastName = createdByUser.LastName;
        CreatedByEmail = createdByUser.Email;
        DisbursementTypeCode = disbursementType.Code;
        DisbursementTypeName = disbursementType.Name;
        AssignToEmail = assignTo;
        AssignCcEmail = assignCC;
    }
}
