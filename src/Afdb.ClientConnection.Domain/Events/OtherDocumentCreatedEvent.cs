using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;

namespace Afdb.ClientConnection.Domain.Events;

public sealed class OtherDocumentCreatedEvent : DomainEvent
{
    public Guid OtherDocumentId { get; }
    public string DocumentName { get; }
    public string Year { get; }
    public string SAPCode { get; }
    public string LoanNumber { get; }
    public string OtherDocumentTypeName { get; }
    public string OtherDocumentTypeNameFr { get; }
    public string CreatedByFirstName { get; }
    public string CreatedByLastName { get; }
    public string CreatedByEmail { get; }
    public string[] FileNames { get; }
    public string[] AssignToEmail { get; }
    public string[] AssignCcEmail { get; }

    public OtherDocumentCreatedEvent(
        Guid otherDocumentId,
        string documentName,
        string year,
        string sapCode,
        string loanNumber,
        OtherDocumentType otherDocumentType,
        User createdByUser,
        string[] fileNames,
        string[] assignToEmail,
        string[] assignCcEmail)
    {
        OtherDocumentId = otherDocumentId;
        DocumentName = documentName;
        Year = year;
        SAPCode = sapCode;
        LoanNumber = loanNumber;
        OtherDocumentTypeName = otherDocumentType.Name;
        OtherDocumentTypeNameFr = otherDocumentType.NameFr;
        CreatedByFirstName = createdByUser.FirstName;
        CreatedByLastName = createdByUser.LastName;
        CreatedByEmail = createdByUser.Email;
        FileNames = fileNames;
        AssignToEmail = assignToEmail;
        AssignCcEmail = assignCcEmail;
    }
}
