using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Domain.Events;
using System.Security.Claims;
using System.Xml.Linq;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Claim : AggregateRoot
{
    private readonly List<ClaimProcess> _processes = [];
    public Guid ClaimTypeId { get; private set; }
    public Guid CountryId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public ClaimStatus Status { get; set; }
    public string Comment { get; private set; }
    public User? User { get; private set; } = default!;
    public ClaimType? ClaimType { get; private set; } = default!;
    public Country? Country { get; private set; } = default!;
    public ICollection<ClaimProcess> Processes => _processes;

    public Claim(ClaimNewParam newParam)
    {
        if (string.IsNullOrWhiteSpace(newParam.Comment))
            throw new ArgumentException("Comment cannot be empty");
        if (newParam.UserId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");
        if (newParam.CountryId == Guid.Empty)
            throw new ArgumentException("CountryId cannot be empty");
        if (newParam.ClaimTypeId == Guid.Empty)
            throw new ArgumentException("ClaimTypeId cannot be empty");

        UserId = newParam.UserId;
        User = newParam.User;
        CountryId = newParam.CountryId;
        Country = newParam.Country;
        ClaimTypeId = newParam.ClaimTypeId;
        Comment = newParam.Comment;
        CreatedAt = DateTime.UtcNow;
        Status = ClaimStatus.Submitted;

        AddDomainEvent(new ClaimCreatedEvent(Id, newParam.Country, newParam.ClaimType,
            User, newParam.AssignTo, newParam.AssignCc, newParam.Comment));
    }

    public Claim(ClaimLoadParam loadParam)
    {
        if (string.IsNullOrWhiteSpace(loadParam.Comment))
            throw new ArgumentException("Comment cannot be empty");
        if (loadParam.UserId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");
        if (loadParam.CountryId == Guid.Empty)
            throw new ArgumentException("CountryId cannot be empty");
        if (loadParam.ClaimTypeId == Guid.Empty)
            throw new ArgumentException("ClaimTypeId cannot be empty");

        Id = loadParam.Id;
        UserId = loadParam.UserId;
        CountryId = loadParam.CountryId;
        ClaimTypeId = loadParam.ClaimTypeId;
        Status = loadParam.Status;
        ClosedAt = loadParam.ClosedAt;
        Comment = loadParam.Comment;
        User = loadParam.User;
        ClaimType = loadParam.ClaimType;
        Country = loadParam.Country;
        CreatedAt = loadParam.CreatedAt;
        UpdatedAt = loadParam.UpdatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedBy = loadParam.UpdatedBy;
        _processes = loadParam.Processes ?? [];
    }

    // Méthodes métier possibles
    public void Close(DateTime closedAt, User user)
    {
        ClosedAt = closedAt;
        Status = ClaimStatus.Closed;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = user.Email;
    }

    public void AddProcess(ClaimProcess process, User user)
    {
        if (process == null)
            throw new ArgumentNullException(nameof(process));
        if (string.IsNullOrWhiteSpace(process.Comment))
            throw new ArgumentException("Comment cannot be empty");

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = user.Email;
        Status = process.Status;
        _processes.Add(process);

        AddDomainEvent(new ClaimProcessAddedEvent(Id, this.Country!, this.ClaimType!,
            this.User!, this.Comment, user, process.Comment, GetClaimStatusString(this.Status)));
    }

    private static string GetClaimStatusString(ClaimStatus status) => status switch
    {
        ClaimStatus.Submitted => "Submitted",
        ClaimStatus.InProgress => "In Progress",
        ClaimStatus.Closed => "Closed",
        _ => "Unknown"
    };
}