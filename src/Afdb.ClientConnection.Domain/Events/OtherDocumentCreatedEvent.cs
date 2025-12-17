using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;

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

    public OtherDocumentCreatedEvent(OtherDocumentEventNewParam newParam)
    {
        OtherDocumentId = newParam.OtherDocumentId;
        DocumentName = newParam.DocumentName;
        Year = newParam.Year;
        SAPCode = newParam.SapCode;
        LoanNumber = newParam.LoanNumber;
        OtherDocumentTypeName = newParam.OtherDocumentType.Name;
        OtherDocumentTypeNameFr = newParam.OtherDocumentType.NameFr;
        CreatedByFirstName = newParam.CreatedByUser.FirstName;
        CreatedByLastName = newParam.CreatedByUser.LastName;
        CreatedByEmail = newParam.CreatedByUser.Email;
        FileNames = newParam.FileNames;
        AssignToEmail = newParam.AssignToEmail;
        AssignCcEmail = newParam.AssignCcEmail;
    }
}
