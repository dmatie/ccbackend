using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Domain.Events;
using Afdb.ClientConnection.Domain.ValueObjects;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Disbursement : AggregateRoot
{
    private readonly List<DisbursementProcess> _processes = [];
    private readonly List<DisbursementDocument> _documents = [];

    public string RequestNumber { get; private set; }
    public string SapCodeProject { get; private set; }
    public string LoanGrantNumber { get; private set; }
    public Guid DisbursementTypeId { get; private set; }
    public Guid CurrencyId { get;private set; }
    public DisbursementStatus Status { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public Guid? ProcessedByUserId { get; private set; }

    public DisbursementType? DisbursementType { get; private set; }
    public Currency? Currency { get; private set; }
    public User? CreatedByUser { get; private set; }
    public User? ProcessedByUser { get; private set; }

    public DisbursementA1? DisbursementA1 { get; private set; }
    public DisbursementA2? DisbursementA2 { get; private set; }
    public DisbursementA3? DisbursementA3 { get; private set; }
    public DisbursementB1? DisbursementB1 { get; private set; }

    public ICollection<DisbursementProcess> Processes => _processes;
    public ICollection<DisbursementDocument> Documents => _documents;

    private Disbursement() { }

    public Disbursement(DisbursementNewParam newParam)
    {
        if (string.IsNullOrWhiteSpace(newParam.SapCodeProject))
            throw new ArgumentException("SapCodeProject cannot be empty");

        if (string.IsNullOrWhiteSpace(newParam.LoanGrantNumber))
            throw new ArgumentException("LoanGrantNumber cannot be empty");

        if (newParam.LoanGrantNumber.Length > 13)
            throw new ArgumentException("LoanGrantNumber cannot exceed 13 characters");

        if (newParam.DisbursementTypeId == Guid.Empty)
            throw new ArgumentException("DisbursementTypeId must be a valid GUID");

        if (newParam.CreatedByUserId == Guid.Empty)
            throw new ArgumentException("CreatedByUserId must be a valid GUID");

        ValidateFormData(newParam);

        RequestNumber = newParam.RequestNumber;
        SapCodeProject = newParam.SapCodeProject;
        LoanGrantNumber = newParam.LoanGrantNumber;
        DisbursementTypeId = newParam.DisbursementTypeId;
        DisbursementType = newParam.DisbursementType;
        Status = DisbursementStatus.Draft;
        CreatedByUserId = newParam.CreatedByUserId;
        CreatedByUser = newParam.CreatedByUser;
        CreatedBy = newParam.CreatedBy;
        CurrencyId = newParam.CurrencyId;

        SetFormData(newParam);

        var process = new DisbursementProcess(new DisbursementProcessNewParam
        {
            Status = DisbursementStatus.Draft,
            ProcessedByUserId = newParam.CreatedByUserId,
            Comment = "Disbursement created",
            CreatedBy = newParam.CreatedBy
        });
        _processes.Add(process);

        AddDomainEvent(new DisbursementCreatedEvent(Id, RequestNumber, SapCodeProject, LoanGrantNumber));
    }

    public Disbursement(DisbursementLoadParam loadParam)
    {
        Id = loadParam.Id;
        RequestNumber = loadParam.RequestNumber;
        SapCodeProject = loadParam.SapCodeProject;
        LoanGrantNumber = loadParam.LoanGrantNumber;
        DisbursementTypeId = loadParam.DisbursementTypeId;
        DisbursementType = loadParam.DisbursementType;
        CurrencyId = loadParam.CurrencyId;
        Currency = loadParam.Currency;
        Status = loadParam.Status;
        CreatedByUserId = loadParam.CreatedByUserId;
        CreatedByUser = loadParam.CreatedByUser;
        SubmittedAt = loadParam.SubmittedAt;
        ProcessedAt = loadParam.ProcessedAt;
        ProcessedByUserId = loadParam.ProcessedByUserId;
        ProcessedByUser = loadParam.ProcessedByUser;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
        UpdatedBy = loadParam.UpdatedBy;
        UpdatedAt = loadParam.UpdatedAt;

        DisbursementA1 = loadParam.DisbursementA1;
        DisbursementA2 = loadParam.DisbursementA2;
        DisbursementA3 = loadParam.DisbursementA3;
        DisbursementB1 = loadParam.DisbursementB1;

        if (loadParam.Processes != null)
            _processes.AddRange(loadParam.Processes);

        if (loadParam.Documents != null)
            _documents.AddRange(loadParam.Documents);
    }

    private static void ValidateFormData(DisbursementNewParam param)
    {
        var typeCode = param.DisbursementType?.Code?.ToUpper();

        switch (typeCode)
        {
            case DisbursementTypeCode.A1:
                if (param.DisbursementA1 == null)
                    throw new ArgumentException("DisbursementA1 data is required for type A1");
                break;
            case DisbursementTypeCode.A2:
                if (param.DisbursementA2 == null)
                    throw new ArgumentException("DisbursementA2 data is required for type A2");
                break;
            case DisbursementTypeCode.A3:
                if (param.DisbursementA3 == null)
                    throw new ArgumentException("DisbursementA3 data is required for type A3");
                break;
            case DisbursementTypeCode.B1:
                if (param.DisbursementB1 == null)
                    throw new ArgumentException("DisbursementB1 data is required for type B1");
                break;
            default:
                throw new ArgumentException($"Invalid disbursement type code: {typeCode}");
        }
    }

    public void ResetFormData()
    {
        DisbursementA1 = null;
        DisbursementA2 = null;
        DisbursementA3 = null;
        DisbursementB1 = null;
    }

    private void SetFormData(DisbursementNewParam param)
    {
        var typeCode = param.DisbursementType?.Code?.ToUpper();

        switch (typeCode)
        {
            case "A1":
                DisbursementA1 = param.DisbursementA1;
                break;
            case "A2":
                DisbursementA2 = param.DisbursementA2;
                break;
            case "A3":
                DisbursementA3 = param.DisbursementA3;
                break;
            case "B1":
                DisbursementB1 = param.DisbursementB1;
                break;
        }
    }

    public void SetFormDataForEdit(string DisbursementTypeCode, 
        DisbursementA1? formA1,
        DisbursementA2? formA2,
        DisbursementA3? formA3,
        DisbursementB1? formB1)
    {

        switch (DisbursementTypeCode)
        {
            case "A1":
                DisbursementA1 = formA1;
                break;
            case "A2":
                DisbursementA2 = formA2;
                break;
            case "A3":
                DisbursementA3 = formA3;
                break;
            case "B1":
                DisbursementB1 = formB1;
                break;
        }
    }


    public void Submit(User user)
    {
        if (Status != DisbursementStatus.Draft && Status != DisbursementStatus.BackedToClient)
            throw new InvalidOperationException("Only draft or backed to client disbursements can be submitted");

        Status = DisbursementStatus.Submitted;
        SubmittedAt = DateTime.UtcNow;
        SetUpdated(user.Email);

        var process = new DisbursementProcess(new DisbursementProcessNewParam
        {
            Status = DisbursementStatus.Submitted,
            ProcessedByUserId = user.Id,
            Comment = "Disbursement submitted for approval",
            CreatedBy = user.Email
        });
        _processes.Add(process);

        AddDomainEvent(new DisbursementSubmittedEvent(Id, RequestNumber, SapCodeProject, LoanGrantNumber));
    }

    public void Resubmit(User user, string comment)
    {
        if (Status != DisbursementStatus.BackedToClient)
            throw new InvalidOperationException("Only backed to client disbursements can be resubmitted");

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required when resubmitting a disbursement");

        Status = DisbursementStatus.Submitted;
        SubmittedAt = DateTime.UtcNow;
        SetUpdated(user.Email);

        var process = new DisbursementProcess(new DisbursementProcessNewParam
        {
            Status = DisbursementStatus.Submitted,
            ProcessedByUserId = user.Id,
            Comment = comment,
            CreatedBy = user.Email
        });
        _processes.Add(process);

        AddDomainEvent(new DisbursementReSubmittedEvent(Id, RequestNumber, SapCodeProject, LoanGrantNumber, comment));
    }

    public void Approve(User user)
    {
        if (Status != DisbursementStatus.Submitted)
            throw new InvalidOperationException("Only submitted disbursements can be approved");

        Status = DisbursementStatus.Approved;
        ProcessedAt = DateTime.UtcNow;
        ProcessedByUserId = user.Id;
        SetUpdated(user.Email);

        var process = new DisbursementProcess(new DisbursementProcessNewParam
        {
            Status = DisbursementStatus.Approved,
            ProcessedByUserId = user.Id,
            Comment = "Disbursement approved",
            CreatedBy = user.Email
        });
        _processes.Add(process);

        AddDomainEvent(new DisbursementApprovedEvent(Id, RequestNumber, SapCodeProject, LoanGrantNumber));
    }

    public void Reject(User user, string comment)
    {
        if (Status != DisbursementStatus.Submitted)
            throw new InvalidOperationException("Only submitted disbursements can be rejected");

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required when rejecting a disbursement");

        Status = DisbursementStatus.Rejected;
        ProcessedAt = DateTime.UtcNow;
        ProcessedByUserId = user.Id;
        SetUpdated(user.Email);

        var process = new DisbursementProcess(new DisbursementProcessNewParam
        {
            Status = DisbursementStatus.Rejected,
            ProcessedByUserId = user.Id,
            Comment = comment,
            CreatedBy = user.Email
        });
        _processes.Add(process);

        AddDomainEvent(new DisbursementRejectedEvent(Id, RequestNumber, SapCodeProject, LoanGrantNumber, comment));
    }

    public void BackToClient(User user, string comment)
    {
        if (Status != DisbursementStatus.Submitted)
            throw new InvalidOperationException("Only submitted disbursements can be backed to client");

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment is required when backing disbursement to client");

        Status = DisbursementStatus.BackedToClient;
        ProcessedAt = DateTime.UtcNow;
        ProcessedByUserId = user.Id;
        SetUpdated(user.Email);

        var process = new DisbursementProcess(new DisbursementProcessNewParam
        {
            Status = DisbursementStatus.BackedToClient,
            ProcessedByUserId = user.Id,
            Comment = comment,
            CreatedBy = user.Email
        });
        _processes.Add(process);

        AddDomainEvent(new DisbursementBackedToClientEvent(Id, RequestNumber, SapCodeProject, LoanGrantNumber, comment, CreatedByUserId));
    }

    public void AddDocument(DisbursementDocument document)
    {
        if (document == null)
            throw new ArgumentException("Document cannot be null");

        _documents.Add(document);
    }

    public bool CanBeSubmitted => Status == DisbursementStatus.Draft || Status == DisbursementStatus.BackedToClient;
    public bool CanBeApproved => Status == DisbursementStatus.Submitted;
    public bool CanBeRejected => Status == DisbursementStatus.Submitted;
    public bool CanBeBackedToClient => Status == DisbursementStatus.Submitted;
    public bool IsProcessed => Status == DisbursementStatus.Approved || Status == DisbursementStatus.Rejected;
}
