using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class OtherDocument : AggregateRoot
{
    private readonly List<OtherDocumentFile> _files = [];

    public Guid OtherDocumentTypeId { get; private set; }
    public string Name { get; private set; }
    public string Year { get; private set; }
    public Guid UserId { get; private set; }
    public OtherDocumentStatus Status { get; private set; }
    public string SAPCode { get; private set; }
    public string LoanNumber { get; private set; }

    public OtherDocumentType? OtherDocumentType { get; private set; }
    public User? User { get; private set; }
    public ICollection<OtherDocumentFile> Files => _files;

    private OtherDocument() { }

    public OtherDocument(OtherDocumentNewParam newParam)
    {
        if (newParam.OtherDocumentTypeId == Guid.Empty)
            throw new ArgumentException("OtherDocumentTypeId must be a valid GUID", nameof(newParam.OtherDocumentTypeId));

        if (string.IsNullOrWhiteSpace(newParam.Name))
            throw new ArgumentException("Name cannot be empty", nameof(newParam.Name));

        if (string.IsNullOrWhiteSpace(newParam.Year))
            throw new ArgumentException("Year cannot be empty", nameof(newParam.Year));

        if (newParam.Year.Length != 4)
            throw new ArgumentException("Year must be 4 characters", nameof(newParam.Year));

        if (newParam.UserId == Guid.Empty)
            throw new ArgumentException("UserId must be a valid GUID", nameof(newParam.UserId));

        if (string.IsNullOrWhiteSpace(newParam.SAPCode))
            throw new ArgumentException("SAPCode cannot be empty", nameof(newParam.SAPCode));

        if (string.IsNullOrWhiteSpace(newParam.LoanNumber))
            throw new ArgumentException("LoanNumber cannot be empty", nameof(newParam.LoanNumber));

        OtherDocumentTypeId = newParam.OtherDocumentTypeId;
        Name = newParam.Name;
        Year = newParam.Year;
        UserId = newParam.UserId;
        Status = newParam.Status;
        SAPCode = newParam.SAPCode;
        LoanNumber = newParam.LoanNumber;
        CreatedBy = newParam.CreatedBy;
    }

    public OtherDocument(OtherDocumentLoadParam loadParam)
    {
        Id = loadParam.Id;
        OtherDocumentTypeId = loadParam.OtherDocumentTypeId;
        Name = loadParam.Name;
        Year = loadParam.Year;
        UserId = loadParam.UserId;
        Status = loadParam.Status;
        SAPCode = loadParam.SAPCode;
        LoanNumber = loadParam.LoanNumber;
        OtherDocumentType = loadParam.OtherDocumentType;
        User = loadParam.User;
        _files = loadParam.Files;
        CreatedAt = loadParam.CreatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }

    public void UpdateStatus(OtherDocumentStatus status, string updatedBy)
    {
        Status = status;
        SetUpdated(updatedBy);
    }

    public void Update(string name, string year, string sapCode, string loanNumber, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(year) || year.Length != 4)
            throw new ArgumentException("Year must be 4 characters", nameof(year));

        if (string.IsNullOrWhiteSpace(sapCode))
            throw new ArgumentException("SAPCode cannot be empty", nameof(sapCode));

        if (string.IsNullOrWhiteSpace(loanNumber))
            throw new ArgumentException("LoanNumber cannot be empty", nameof(loanNumber));

        Name = name;
        Year = year;
        SAPCode = sapCode;
        LoanNumber = loanNumber;
        SetUpdated(updatedBy);
    }

    public void AddFile(OtherDocumentFile file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        _files.Add(file);
    }

    public void Submit(string updatedBy)
    {
        if (Status != OtherDocumentStatus.Draft)
            throw new InvalidOperationException("Only draft documents can be submitted");

        Status = OtherDocumentStatus.Submitted;
        SetUpdated(updatedBy);
    }

    public void MarkAsConsulted(string updatedBy)
    {
        if (Status != OtherDocumentStatus.Submitted)
            throw new InvalidOperationException("Only submitted documents can be marked as consulted");

        Status = OtherDocumentStatus.Consulted;
        SetUpdated(updatedBy);
    }
}
