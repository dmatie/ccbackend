using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class DisbursementBackedToClientEvent : DomainEvent
{
    public Guid DisbursementId { get; }
    public string RequestNumber { get; }
    public string SapCodeProject { get; }
    public string LoanGrantNumber { get; }
    public string Comment { get; }
    public string CreatedByFirstName { get; }
    public string CreatedByLastName { get; }
    public string CreatedByEmail { get; }
    public string ProcessedByFirstName { get; }
    public string ProcessedByLastName { get; }
    public string ProcessedByEmail { get; }
    public string DisbursementTypeCode { get; }
    public string DisbursementTypeName { get; }

    public DisbursementBackedToClientEvent(
        Guid disbursementId,
        string requestNumber,
        string sapCodeProject,
        string loanGrantNumber,
        string comment,
        User createdByUser,
        User processedByUser,
        DisbursementType disbursementType)
    {
        DisbursementId = disbursementId;
        RequestNumber = requestNumber;
        SapCodeProject = sapCodeProject;
        LoanGrantNumber = loanGrantNumber;
        Comment = comment;
        CreatedByFirstName = createdByUser.FirstName;
        CreatedByLastName = createdByUser.LastName;
        CreatedByEmail = createdByUser.Email;
        ProcessedByFirstName = processedByUser.FirstName;
        ProcessedByLastName = processedByUser.LastName;
        ProcessedByEmail = processedByUser.Email;
        DisbursementTypeCode = disbursementType.Code;
        DisbursementTypeName = disbursementType.Name;
    }
}
