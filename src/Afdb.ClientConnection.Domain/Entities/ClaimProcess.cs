using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class ClaimProcess : AggregateRoot
{
    public Guid ClaimId { get; private set; }
    public Guid UserId { get; private set; }
    public ClaimStatus Status { get; set; }
    public string Comment { get; private set; }
    public User User { get; private set; }= default!;

    public ClaimProcess(ClaimProcessNewParam newParam )
    {
        if (string.IsNullOrWhiteSpace(newParam.Comment))
            throw new ArgumentException("Comment cannot be empty");

        ClaimId = newParam.ClaimId;
        UserId = newParam.UserId;
        Status = newParam.Status;
        Comment = newParam.Comment;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = newParam.User.Email;

    }
    public ClaimProcess(ClaimProcessLoadParam loadParam)
    {
        Id = loadParam.Id;
        ClaimId = loadParam.ClaimId;
        UserId = loadParam.UserId;
        Status = loadParam.Status;
        User = loadParam.User;
        Comment = loadParam.Comment;
        CreatedAt = loadParam.CreatedAt;
        UpdatedAt = loadParam.UpdatedAt;
        CreatedBy = loadParam.CreatedBy;
        UpdatedBy = loadParam.UpdatedBy;
    }
}